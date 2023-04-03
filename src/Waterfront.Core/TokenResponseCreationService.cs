using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens;
using Waterfront.Core.Configuration;
using Waterfront.Core.Utility.Serialization.Acl;

namespace Waterfront.Core;

public class TokenResponseCreationService : ITokenResponseCreationService
{
    private readonly ILogger<TokenResponseCreationService> _logger;
    private readonly IOptions<TokenOptions>                _tokenOptions;

    public TokenResponseCreationService(
        ILogger<TokenResponseCreationService> logger,
        IOptions<TokenOptions> tokenOptions
    )
    {
        _logger       = logger;
        _tokenOptions = tokenOptions;
    }

    public ValueTask<TokenResponse> CreateResponseAsync(
        TokenRequest request,
        TokenRequestAuthenticationResult authenticationResult,
        TokenRequestAuthorizationResult authorizationResult
    )
    {
        _logger.LogDebug("Creating TokenResponse for request {RequestId}", request.Id);

        string issuer   = _tokenOptions.Value.Issuer;
        var    lifetime = _tokenOptions.Value.Lifetime;

        var issuedAt  = DateTimeOffset.UtcNow;
        var expiresAt = issuedAt.Add(lifetime);

        var subject = authenticationResult.User!.Username;

        if (request.IsAuthenticationRequest)
        {
            return ValueTask.FromResult(
                new TokenResponse {
                    IsSuccessful = authenticationResult.IsSuccessful,
                    Access       = Array.Empty<TokenResponseAccessEntry>(),
                    Subject      = subject,
                    Issuer       = issuer,
                    Service      = request.Service,
                    IssuedAt     = issuedAt
                }
            );
        }

        IEnumerable<TokenResponseAccessEntry> accessEntryList;

        if (!request.IsAuthenticationRequest)
        {
            accessEntryList = from authorizedScope in authorizationResult.AuthorizedScopes
                              select new TokenResponseAccessEntry {
                                  Type = authorizedScope.Type.ToSerialized(),
                                  Name = authorizedScope.Name,
                                  Actions = from action in authorizedScope.Actions
                                            select action.ToSerialized()
                              };
        }
        else
        {
            accessEntryList = Array.Empty<TokenResponseAccessEntry>();
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
