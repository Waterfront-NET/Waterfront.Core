using System.Threading.Tasks;
using Waterfront.Common.Authentication;
using Waterfront.Common.Tokens;

namespace Waterfront.Core.Authentication;

public interface IAclAuthenticationService
{
    ValueTask<TokenRequestAuthenticationResult> AuthenticateAsync(TokenRequest request);
}