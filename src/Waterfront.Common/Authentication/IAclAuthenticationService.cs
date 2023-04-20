using System.Threading.Tasks;
using Waterfront.Common.Tokens;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Common.Authentication;

public interface IAclAuthenticationService
{
    /// <summary>
    /// Attempts to authenticate given <see cref="TokenRequest"/> using internal logic
    /// </summary>
    /// <param name="request">Request to authenticate</param>
    /// <returns>Authentication result</returns>
    ValueTask<AclAuthenticationResult> AuthenticateAsync(TokenRequest request);
}
