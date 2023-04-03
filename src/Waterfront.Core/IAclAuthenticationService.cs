using System.Threading.Tasks;
using Waterfront.Common.Authentication;
using Waterfront.Common.Tokens;

namespace Waterfront.Core;

public interface IAclAuthenticationService
{
    ValueTask<TokenRequestAuthenticationResult> AuthenticateAsync(TokenRequest request);
}