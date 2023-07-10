﻿using Microsoft.Extensions.Logging;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens.Definition;
using Waterfront.Common.Tokens.Requests;
using Waterfront.Core.Configuration.Tokens;

namespace Waterfront.Core.Tokens.Definition;

public class TokenDefinitionService : ITokenDefinitionService
{
    protected ILogger Logger { get; }
    protected ITokenOptionsProvider TokenOptionsProvider { get; }

    public TokenDefinitionService(
        ILoggerFactory loggerFactory,
        ITokenOptionsProvider tokenOptionsProvider
    )
    {
        Logger               = loggerFactory.CreateLogger(GetType());
        TokenOptionsProvider = tokenOptionsProvider;
    }

    public virtual async ValueTask<TokenDefinition> CreateDefinitionAsync(
        TokenRequest request,
        AclAuthenticationResult authenticationResult,
        AclAuthorizationResult authorizationResult
    )
    {
        TokenOptions   options   = await TokenOptionsProvider.GetTokenOptionsAsync(request);
        DateTimeOffset issuedAt  = await GetIssuedAtAsync(request);
        DateTimeOffset expiresAt = await GetExpiresAtAsync(request, issuedAt);

        if ( !authenticationResult.IsSuccessful )
        {
            throw new InvalidOperationException(
                "Cannot create tokenDefinition for request failed to authenticate"
            ) { Data = { { "request_id", request.Id } } };
        }

        if ( !authorizationResult.IsSuccessful )
        {
            throw new InvalidOperationException(
                "Cannot create tokenDefinition for request failed to authorize"
            ) { Data = { { "request_id", request.Id } } };
        }

        TokenDefinition definition = new TokenDefinition {
            Id = request.Id,
            Subject =
            authenticationResult.User.Username,
            Issuer    = options.Issuer,
            Service   = request.Service,
            IssuedAt  = issuedAt,
            ExpiresAt = expiresAt,
            Access =
            authorizationResult.AuthorizedScopes.ToArray()
        };

        return definition;
    }

    protected virtual ValueTask<DateTimeOffset> GetIssuedAtAsync(TokenRequest request) =>
    ValueTask.FromResult(DateTimeOffset.Now);

    protected virtual async ValueTask<DateTimeOffset> GetExpiresAtAsync(
        TokenRequest request,
        DateTimeOffset issuedAt
    ) => issuedAt.Add((await TokenOptionsProvider.GetTokenOptionsAsync(request)).Lifetime);
}
