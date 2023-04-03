using System.Threading.Tasks;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens;

namespace Waterfront.Core;

public interface ITokenResponseCreationService
{
    ValueTask<TokenResponse> CreateResponseAsync(
        TokenRequest request,
        TokenRequestAuthenticationResult authenticationResult,
        TokenRequestAuthorizationResult authorizationResult
    );
}