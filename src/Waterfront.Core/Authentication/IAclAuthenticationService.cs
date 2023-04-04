using System.Threading.Tasks;
using Waterfront.Common.Authentication;
using Waterfront.Common.Tokens;

namespace Waterfront.Core.Authentication;

public interface IAclAuthenticationService
{
    /// <summary>
    /// Attempts to authenticate given <see cref="TokenRequest"/> using internal logic
    /// </summary>
    /// <param name="request">Request to authenticate</param>
    /// <returns>Authentication result</returns>
    ValueTask<TokenRequestAuthenticationResult> AuthenticateAsync(TokenRequest request);
}