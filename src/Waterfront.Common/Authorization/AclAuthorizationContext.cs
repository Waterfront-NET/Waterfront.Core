using Waterfront.Common.Authentication;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Common.Authorization;

public class AclAuthorizationContext
{
    public AclAuthenticationResult AuthenticationResult { get; }

    public ICollection<TokenRequestScope> PendingScopes { get; }
    public ICollection<TokenRequestScope> AuthorizedScopes { get; }
    public ICollection<TokenRequestScope> ForbiddenScopes { get; }


}
