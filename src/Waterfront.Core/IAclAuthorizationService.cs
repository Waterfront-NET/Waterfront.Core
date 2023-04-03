using System.Threading.Tasks;
using Waterfront.Common.Acl;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens;

namespace Waterfront.Core;

public interface IAclAuthorizationService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    ValueTask<TokenRequestAuthorizationResult> AuthorizeAsync(
        TokenRequest request,
        AclUser user
    );
}
