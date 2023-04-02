using Waterfront.Common.Authentication;
using Waterfront.Common.Tokens;

namespace Waterfront.Core;

public interface IAclAuthenticationService
{
    Task<TokenRequestAuthenticationResult> AuthenticateAsync(TokenRequest request);
}