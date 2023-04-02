using Waterfront.Common.Acl;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens;

namespace Waterfront.Core;

public interface IAclAuthorizationService
{
    Task<TokenRequestAuthorizationResult> AuthorizeAsync(TokenRequest request, AclUser user);
}