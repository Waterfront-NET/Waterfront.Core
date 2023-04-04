using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.AspNetCore.Configuration;
using Waterfront.AspNetCore.Configuration.Endpoints;
using Waterfront.AspNetCore.Extensions;
using Waterfront.AspNetCore.Extensions.Tokens;
using Waterfront.AspNetCore.Json.Converters;
using Waterfront.AspNetCore.Services.Authentication;
using Waterfront.AspNetCore.Services.Authorization;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Common.Contracts.Tokens.Response;
using Waterfront.Common.Tokens;
using Waterfront.Core;
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
    }

    public async Task InvokeAsync(
        HttpContext context,
        ITokenRequestService tokenRequestCreationService,
        ITokenDefinitionService tokenResponseCreationService,
        ITokenEncoder tokenEncoder,
        TokenRequestAuthenticationService tokenRequestAuthenticationService,
        TokenRequestAuthorizationService tokenRequestAuthorizationService
    )
    {
        if ( context.Request.Path == _endpointOptions.Value.TokenEndpoint )
        {
            await InvokeTokenEndpointAsync(
                context,
                tokenRequestCreationService,
                tokenRequestAuthenticationService,
                tokenRequestAuthorizationService,
                tokenResponseCreationService,
                tokenEncoder
            );
        }
        else if ( context.Request.Path == _endpointOptions.Value.InfoEndpoint )
        {
            await InvokeInfoEndpointAsync(context);
        }

        else if ( context.Request.Path == _endpointOptions.Value.PublicKeyEndpoint )
        {
            await InvokePublicKeyEndpointAsync(context);
        }
        else
        {
            await _next(context);
        }
    }

    private async Task InvokeTokenEndpointAsync(
        HttpContext context,
        ITokenRequestService tokenRequestService,
        TokenRequestAuthenticationService tokenRequestAuthenticationService,
        TokenRequestAuthorizationService tokenRequestAuthorizationService,
        ITokenDefinitionService tokenDefinitionService,
        ITokenEncoder tokenEncoder
    )
    {
        (bool tokenRequestCreated, TokenRequest tokenRequest) =
        await tokenRequestService.TryCreateRequestAsync(context);

        if ( !tokenRequestCreated )
        {
            await Results.BadRequest(new { message = "Invalid request params" })
                         .ExecuteAsync(context);
            return;
        }

        TokenRequestAuthenticationResult authNResult =
        await tokenRequestAuthenticationService.AuthenticateAsync(tokenRequest);

        if ( !authNResult.IsSuccessful )
        {
            _logger.LogWarning("Failed to authenticate request {RequestId}", tokenRequest.Id);
            await context.Response.WriteAsJsonAsync(
                new {
                    statusCode = 401,
                    message    = "Failed to authenticate"
                }
            );
            context.Response.StatusCode = HttpStatusCode.Unauthorized.ToInt32();
            return;
        }

        TokenRequestAuthorizationResult authZResult =
        await tokenRequestAuthorizationService.AuthorizeAsync(tokenRequest, authNResult);

        if ( !authZResult.IsSuccessful )
        {
            _logger.LogWarning(
                "Failed to authorize request {RequestId}, authorization failed for the following scopes: {@ForbiddenScopes}",
                tokenRequest.Id,
                authZResult.ForbiddenScopes
            );
            await context.Response.WriteAsJsonAsync(
                new {
                    message         = "Failed to authorize",
                    forbiddenScopes = authZResult.ForbiddenScopes,
                    statusCode      = 401
                }
            );
            context.Response.StatusCode = HttpStatusCode.Unauthorized.ToInt32();
            return;
        }

        TokenDefinition tokenDefinition =
        await tokenDefinitionService.CreateTokenDefinitionAsync(
            tokenRequest,
            authNResult,
            authZResult
        );

        string jwToken = await tokenEncoder.EncodeTokenAsync(tokenDefinition);

        TokenResponse tokenDto = new TokenResponse {
            Token = jwToken,
            IssuedAt = tokenDefinition.IssuedAt.ToString("O"),
            ExpiresIn = (int) (tokenDefinition.ExpiresAt - tokenDefinition.IssuedAt).TotalSeconds,
            RefreshToken = null
        };

        await context.Response.WriteAsJsonAsync(
            tokenDto,
            new JsonSerializerOptions { Converters = { TokenResponseJsonConverter.Instance } }
        );
        context.Response.StatusCode = HttpStatusCode.OK.ToInt32();
    }

    private Task InvokeInfoEndpointAsync(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private Task InvokePublicKeyEndpointAsync(HttpContext context)
    {
        throw new NotImplementedException();
    }
}
