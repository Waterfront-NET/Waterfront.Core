using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.AspNetCore.Configuration;
using Waterfront.AspNetCore.Extensions;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens;
using Waterfront.Core;
using Waterfront.Core.Authentication;
using Waterfront.Core.Authorization;
using Waterfront.Core.Jwt;
using Waterfront.Core.Utility.Parsing;

namespace Waterfront.AspNetCore.Middleware;

public class WaterfrontMiddleware
{
    private readonly ILogger<WaterfrontMiddleware>       _logger;
    private readonly IOptions<WaterfrontEndpointOptions> _endpointOptions;
    private readonly RequestDelegate                     _next;

    public WaterfrontMiddleware(
        ILogger<WaterfrontMiddleware> logger,
        IOptions<WaterfrontEndpointOptions> endpointOptions,
        RequestDelegate next
    )
    {
        _logger          = logger;
        _endpointOptions = endpointOptions;
        _next            = next;
        logger.LogInformation("Waterfront middleware initialized");
    }

    public async Task InvokeAsync(
        HttpContext context,
        ITokenRequestCreationService tokenRequestCreationService,
        IEnumerable<IAclAuthenticationService> authenticationServices,
        IEnumerable<IAclAuthorizationService> authorizationServices,
        ITokenResponseCreationService tokenResponseCreationService,
        ITokenCreationService tokenCreationService
    )
    {
        _logger.LogInformation("Waterfront middleware invoke");
        if (context.Request.Path == _endpointOptions.Value.TokenEndpoint)
        {
            IQueryCollection query = context.Request.Query;

            if (!QueryParamResolver.TryGetQueryParams(
                    query,
                    out string service,
                    out string? account,
                    out string? clientId,
                    out string? offlineToken,
                    out IEnumerable<string> scopes
                ))
            {
                BadRequestObjectResult result =
                new BadRequestObjectResult(new { message = "Invalid query string" });

                await context.Response.WriteAsJsonAsync(result);
                return;
            }

            TokenRequest tokenRequest = await tokenRequestCreationService.CreateRequestAsync(
                                            service,
                                            context.Connection.RemoteIpAddress!,
                                            context.Connection.RemotePort,
                                            account,
                                            clientId,
                                            PrimitiveParser.ParseBoolean(offlineToken),
                                            scopes,
                                            context.Request.Headers.Authorization.ToString(),
                                            null
                                        );

            TokenRequestAuthenticationResult authnResult = TokenRequestAuthenticationResult.Failed;

            foreach (IAclAuthenticationService authenticationService in authenticationServices)
            {
                TokenRequestAuthenticationResult authnResultTemp =
                await authenticationService.AuthenticateAsync(tokenRequest);

                if (authnResultTemp.IsSuccessful)
                {
                    authnResult = authnResultTemp;
                    break;
                }
            }

            if (!authnResult.IsSuccessful)
            {
                _logger.LogError("Failed to authenticate");
                await context.Response.WriteAsJsonAsync(new UnauthorizedResult());
                return;
            }

            _logger.LogInformation(
                "Authentication successful:\n{@AuthenticationResult}",
                authnResult
            );

            TokenRequestAuthorizationResult authzResult = new TokenRequestAuthorizationResult {
                AuthorizedScopes = Array.Empty<TokenRequestScope>(),
                ForbiddenScopes  = tokenRequest.Scopes
            };

            foreach (IAclAuthorizationService authorizationService in authorizationServices)
            {
                TokenRequestAuthorizationResult authzResultTemp =
                await authorizationService.AuthorizeAsync(tokenRequest, authnResult.User!);

                if (authzResultTemp.IsSuccessful)
                {
                    authzResult = authzResultTemp;
                    break;
                }

                IEnumerable<TokenRequestScope> allAuthzed =
                authzResult.AuthorizedScopes.Concat(authzResultTemp.AuthorizedScopes);

                authzResult = new TokenRequestAuthorizationResult {
                    AuthorizedScopes = allAuthzed,
                    ForbiddenScopes =
                    tokenRequest.Scopes.Where(scope => !allAuthzed.Contains(scope))
                };

                if (authzResult.IsSuccessful)
                    break;
            }

            if (!authzResult.IsSuccessful)
            {
                await context.Response.WriteAsJsonAsync(new UnauthorizedResult());
                return;
            }

            TokenResponse tokenResponse = await tokenResponseCreationService.CreateResponseAsync(
                                              tokenRequest,
                                              authnResult,
                                              authzResult
                                          );

            string token = await tokenCreationService.CreateTokenAsync(tokenResponse);

            var responseRaw = new { token };

            await context.Response.WriteAsJsonAsync(responseRaw);
            return;
        }

        if (context.Request.Path == _endpointOptions.Value.InfoEndpoint)
        {
            throw new NotImplementedException();
        }

        if (context.Request.Path == _endpointOptions.Value.PublicKeyEndpoint)
        {
            throw new NotImplementedException();
        }

        await _next(context);
    }

    private async Task InvokeTokenEndpoint(HttpContext context) { }

    private Task InvokeInfoEndpoint(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private Task InvokePublicKeyEndpoint(HttpContext context)
    {
        throw new NotImplementedException();
    }
}
