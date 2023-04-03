using System.Collections.Generic;
using System.Linq;
using Waterfront.Common.Tokens;

namespace Waterfront.Common.Authorization;

public readonly struct TokenRequestAuthorizationResult
{
    public bool IsSuccessful => !ForbiddenScopes.Any();
    public IEnumerable<TokenRequestScope> AuthorizedScopes { get; init; }
    public IEnumerable<TokenRequestScope> ForbiddenScopes { get; init; }
}
