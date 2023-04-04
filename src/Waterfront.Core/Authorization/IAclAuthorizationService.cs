using System.Threading.Tasks;
using Waterfront.Common.Acl;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens;

namespace Waterfront.Core.Authorization;

public interface IAclAuthorizationService
{
    /// <summary>
    /// Attempts to authorize given <see cref="TokenRequest"/> using given <see cref="AclUser"/> entity
    /// </summary>
    /// <param name="request">Request to authorize</param>
    /// <param name="authnResult"></param>
    /// <param name="authzResult"></param>
    /// <returns>Operation result</returns>
    ValueTask<TokenRequestAuthorizationResult> AuthorizeAsync(
        TokenRequest request,
        TokenRequestAuthenticationResult authnResult,
        TokenRequestAuthorizationResult authzResult
    );
}
