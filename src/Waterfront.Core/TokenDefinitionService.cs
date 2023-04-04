using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens;
using Waterfront.Core.Configuration;
using Waterfront.Core.Configuration.Tokens;
using Waterfront.Core.Extensions;

namespace Waterfront.Core;

public class TokenDefinitionService : ITokenDefinitionService
{
    private readonly ILogger<TokenDefinitionService> _logger;
    private readonly IOptions<TokenOptions>                _tokenOptions;

    public TokenDefinitionService(
        ILogger<TokenDefinitionService> logger,
        IOptions<TokenOptions> tokenOptions
    )
    {
        _logger       = logger;
        _tokenOptions = tokenOptions;
    }

    public ValueTask<TokenDefinition> CreateTokenDefinitionAsync(
        TokenRequest request,
        TokenRequestAuthenticationResult authenticationResult,
        TokenRequestAuthorizationResult authorizationResult
    )
    {
        DateTimeOffset issuedAt = DateTimeOffset.UtcNow;
        _logger.LogDebug(
            "Creating TokenResponse for request {RequestId} at {IssuedAt}",
            request.Id,
            issuedAt
        );

        TokenDefinition response = new TokenDefinition {
            Id = request.Id,
            Subject =
            authenticationResult.User?.Username ??
            throw new InvalidOperationException("Cannot create token response for null user"),
            Issuer    = _tokenOptions.Value.Issuer,
            Service   = request.Service,
            IssuedAt  = issuedAt,
            ExpiresAt = issuedAt.Add(_tokenOptions.Value.Lifetime),
            Access = from authorizedScope in authorizationResult.AuthorizedScopes
                     select authorizedScope.ToTokenResponseAccessEntry()
        };

        _logger.LogDebug("Response: {@Response}", response);

        return ValueTask.FromResult(response);
    }
}
