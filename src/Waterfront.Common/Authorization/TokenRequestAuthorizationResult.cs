using System;
using System.Collections.Generic;
using System.Linq;
using Waterfront.Common.Tokens;

namespace Waterfront.Common.Authorization;

public readonly struct TokenRequestAuthorizationResult
{
    public bool IsSuccessful => !ForbiddenScopes.Any();
    public IEnumerable<TokenRequestScope> AuthorizedScopes { get; init; }
    public IEnumerable<TokenRequestScope> ForbiddenScopes { get; init; }

    public TokenRequestAuthorizationResult()
    {
        AuthorizedScopes = Array.Empty<TokenRequestScope>();
        ForbiddenScopes  = Array.Empty<TokenRequestScope>();
    }
    
    public TokenRequestAuthorizationResult WithAuthorizedScopes(
        IEnumerable<TokenRequestScope> scopes
    )
    {
        TokenRequestScope[]            asList = AuthorizedScopes.Union(scopes).ToArray();
        IEnumerable<TokenRequestScope> fsList =
        ForbiddenScopes.Where(scope => !asList.Contains(scope));

        return new TokenRequestAuthorizationResult {
            AuthorizedScopes = asList,
            ForbiddenScopes  = fsList
        };
    }
}
