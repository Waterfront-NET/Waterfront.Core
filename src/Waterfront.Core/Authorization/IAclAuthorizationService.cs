using System.Threading.Tasks;
using Waterfront.Common.Acl;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens;

namespace Waterfront.Core.Authorization;

public interface IAclAuthorizationService
{
    /// <summary>
    /// Attempts to authorize given <see cref="TokenRequest"/> using given <see cref="AclUser"/> entity
    /// </summary>
    /// <param name="request">Request to authorize</param>
    /// <param name="user">User to fetch acl policies from</param>
    /// <returns>Operation result</returns>
    ValueTask<TokenRequestAuthorizationResult> AuthorizeAsync(
        TokenRequest request,
        AclUser user
    );
}
