using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens.Definition;
using Waterfront.Common.Tokens.Requests;
using Waterfront.Core.Configuration.Tokens;
using Waterfront.Core.Serialization.Acl;

namespace Waterfront.Core.Tokens.Definition;

public class TokenDefinitionService : ITokenDefinitionService
{
    private readonly ILogger<TokenDefinitionService> _logger;
    private readonly IOptions<TokenOptions> _tokenOptions;

    public TokenDefinitionService(
        ILogger<TokenDefinitionService> logger,
        IOptions<TokenOptions> tokenOptions
    )
    {
        _logger = logger;
        _tokenOptions = tokenOptions;
    }

    public ValueTask<TokenDefinition> CreateDefinitionAsync(
        TokenRequest request,
        AclAuthenticationResult authenticationResult,
        AclAuthorizationResult authorizationResult
    )
    {
        DateTimeOffset issuedAt = DateTimeOffset.UtcNow;
        DateTimeOffset expiresAt = issuedAt.Add(_tokenOptions.Value.Lifetime);

        _logger.LogDebug(
            "Creating TokenDefinition for request {RequestId} at {IssuedAt}",
            request.Id,
            issuedAt
        );

        if (!authenticationResult.IsSuccessful)
        {
            throw new InvalidOperationException(
                "Cannot create tokenDefinition for request failed to authenticate"
            )
            {
                Data = { { "request_id", request.Id } }
            };
        }

        if (!authorizationResult.IsSuccessful)
        {
            throw new InvalidOperationException(
                "Cannot create tokenDefinition for request failed to authorize"
            )
            {
                Data = { { "request_id", request.Id } }
            };
        }

        _logger.LogDebug(
            "Subject: {Subject}\n"
                + "Issuer: {Issuer}\n"
                + "Service: {Service}\n"
                + "IssuedAt: {IssuedAt}\n"
                + "ExpiresAt: {ExpiresAt}\n"
                + "Access: {Access}",
            authenticationResult.User.Username,
            _tokenOptions.Value.Issuer,
            request.Service,
            issuedAt.ToString("O"),
            expiresAt.ToString("O"),
            $"[{string.Join(",", authorizationResult.AuthorizedScopes.Select(scope => scope.ToSerialized()))}]"
        );

        TokenDefinition definition = new TokenDefinition
        {
            Id = request.Id,
            Subject = authenticationResult.User.Username,
            Issuer = _tokenOptions.Value.Issuer,
            Service = request.Service,
            IssuedAt = issuedAt,
            ExpiresAt = expiresAt,
            Access = authorizationResult.AuthorizedScopes
        };

        // _logger.LogDebug("TokenDefinition: {@Definition}", definition);

        return ValueTask.FromResult(definition);
    }
}
