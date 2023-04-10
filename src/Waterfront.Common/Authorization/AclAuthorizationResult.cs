using System;
using System.Collections.Generic;
using System.Linq;
using Waterfront.Common.Authentication;
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

    /// <summary>
    /// Creates a copy of current result, merged with <paramref name="other"/>, containing authorized scopes of both
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public AclAuthorizationResult MergeWith(AclAuthorizationResult other)
    {
        TokenRequestScope[] allAuthorizedScopes =
        AuthorizedScopes.Union(other.AuthorizedScopes).ToArray();
        TokenRequestScope[] remainingForbiddenScopes =
        ForbiddenScopes.Except(allAuthorizedScopes).ToArray();

        return new AclAuthorizationResult {
            AuthorizedScopes = allAuthorizedScopes,
            ForbiddenScopes  = remainingForbiddenScopes
        };
    }
}
