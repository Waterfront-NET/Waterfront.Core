using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens;
using Waterfront.Core.Utility.Serialization.Acl;

namespace Waterfront.Core;

public class TokenResponseCreationService : ITokenResponseCreationService
{
    private readonly ILogger<TokenResponseCreationService> _logger;

    public TokenResponseCreationService(ILogger<TokenResponseCreationService> logger)
    {
        _logger = logger;
    }

    public ValueTask<TokenResponse> CreateResponseAsync(
        TokenRequest request,
        TokenRequestAuthenticationResult authenticationResult,
        TokenRequestAuthorizationResult authorizationResult
    )
    {
        _logger.LogDebug("Creating TokenResponse for request {RequestId}", request.Id);

        if (request.IsAuthenticationRequest)
        {
            return ValueTask.FromResult(
                new TokenResponse {
                    IsSuccessful = authenticationResult.IsSuccessful,
                    Access       = Array.Empty<TokenResponseAccessEntry>(),
                    
                }
            );
        }

        if (!request.IsAuthenticationRequest)
        {
            foreach (TokenRequestScope scope in authorizationResult.AuthorizedScopes)
            {
                var tokenResponseAccessEntry = new TokenResponseAccessEntry {
                    Type    = scope.Type.ToSerialized(),
                    Name    = scope.Name,
                    Actions = scope.Actions.Select(action => action.ToSerialized())
                };
                
                
            }
        }

        throw new NotImplementedException();

        // var response = new TokenResponse {
        //     Subject = authenticationResult.User!.Username,
        //     Service = request.Service,
        //     IsSuccessful = !authorizationResult.ForbiddenScopes.Any(),
        //     
        // }
    }

    // private IEnumerable<TokenResponseAccessEntry> CreateAccess()
}