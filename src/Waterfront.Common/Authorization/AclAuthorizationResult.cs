using System;
using System.Collections.Generic;
using System.Linq;
using Waterfront.Common.Tokens;

namespace Waterfront.Common.Authorization;

public readonly struct AclAuthorizationResult
{
    public bool IsSuccessful => !ForbiddenScopes.Any();
    public IReadOnlyList<TokenRequestScope> AuthorizedScopes { get; init; }
    public IReadOnlyList<TokenRequestScope> ForbiddenScopes { get; init; }

    public AclAuthorizationResult()
    {
        AuthorizedScopes = Array.Empty<TokenRequestScope>();
        ForbiddenScopes  = Array.Empty<TokenRequestScope>();
    }

    public AclAuthorizationResult WithAuthorizedScopes(
        IEnumerable<TokenRequestScope> scopes
    )
    {
        TokenRequestScope[] newAs = AuthorizedScopes.Union(scopes).ToArray();
        return new AclAuthorizationResult {
            AuthorizedScopes = newAs,
            ForbiddenScopes  = ForbiddenScopes.Except(newAs).ToArray()
        };
    }
}
