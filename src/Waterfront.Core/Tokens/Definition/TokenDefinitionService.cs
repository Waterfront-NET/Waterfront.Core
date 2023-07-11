using Microsoft.Extensions.Logging;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Common.Configuration;
using Waterfront.Common.Tokens.Configuration;
using Waterfront.Common.Tokens.Definition;
using Waterfront.Common.Tokens.Requests;
using Waterfront.Core.Extensions.DependencyInjection;

namespace Waterfront.Core.Tokens.Definition;

public class TokenDefinitionService : ITokenDefinitionService
{
    protected ILogger Logger { get; }
    protected IServiceOptionsProvider<TokenOptions> TokenOptionsProvider { get; }

    public TokenDefinitionService(
        ILoggerFactory loggerFactory,
        IServiceOptionsProvider<TokenOptions> tokenOptionsProvider
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
        TokenOptions   options   = TokenOptionsProvider.Get(request.Service);
        DateTimeOffset issuedAt  = DateTimeOffset.Now;
        DateTimeOffset expiresAt = issuedAt.Add(options.Lifetime);

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
            Id        = request.Id,
            Subject   = authenticationResult.User.Username,
            Issuer    = options.Issuer,
            Service   = request.Service,
            IssuedAt  = issuedAt,
            ExpiresAt = expiresAt,
            Access    = authorizationResult.AuthorizedScopes.ToArray()
        };

        return definition;
    }
}
