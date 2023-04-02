using System.Collections.Generic;
using System.Linq;
using Waterfront.Common.Tokens;

namespace Waterfront.Common.Authorization;

public class TokenRequestAuthorizationResult
{
    public bool IsSuccessful => !ForbiddenScopes.Any();
    public IEnumerable<TokenRequestScope> AuthorizedScopes { get; }
    public IEnumerable<TokenRequestScope> ForbiddenScopes { get; }
}