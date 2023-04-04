using Microsoft.Extensions.Logging;
using Waterfront.Common.Acl;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens;
using Waterfront.Core.Authorization;

namespace Waterfront.AspNetCore.Services.Authorization;

public class TokenRequestAuthorizationService
{
    private readonly ILogger<TokenRequestAuthorizationService> _logger;
    private readonly IEnumerable<IAclAuthorizationService>     _authorizationServices;

    public TokenRequestAuthorizationService(ILogger<TokenRequestAuthorizationService> logger, IEnumerable<IAclAuthorizationService> authorizationServices)
    {
        _logger                     = logger;
        _authorizationServices = authorizationServices;
    }

    public async ValueTask<TokenRequestAuthorizationResult> AuthorizeAsync(
        TokenRequest request,
        TokenRequestAuthenticationResult authnResult
    )
    {
        if (request.IsAuthenticationRequest)
        {
            return new TokenRequestAuthorizationResult {
                AuthorizedScopes = Array.Empty<TokenRequestScope>(),
                ForbiddenScopes  = Array.Empty<TokenRequestScope>()
            };
        }
        
        if (!authnResult.IsSuccessful)
        {
            throw new InvalidOperationException($"Cannot authorize unauthenticated request ({request.Id})");
        }

        AclUser user = authnResult.User!;

        var result = new TokenRequestAuthorizationResult { ForbiddenScopes = request.Scopes };
        
        foreach (IAclAuthorizationService service in _authorizationServices)
        {
            var currentResult = await service.AuthorizeAsync(request, user);

            _logger.LogInformation(
                "Current result: {@CurrentResult}\nOld result: {@OldResult}",
                currentResult,
                result
            );

            result = result.WithAuthorizedScopes(currentResult.AuthorizedScopes);

            _logger.LogInformation("Mutated result: {@MutResult}", result);
            if (result.IsSuccessful)
            {
                break;
            }
        }

        if (!result.IsSuccessful)
        {
            _logger.LogWarning(
                "Failed to authorize scopes {@Scopes} on request {RequestId}",
                result.ForbiddenScopes,
                request.Id
            );
        }
        else
        {
            _logger.LogInformation("Request {RequestId} was successfully authorized", request.Id);
        }

        return result;
    }
}
