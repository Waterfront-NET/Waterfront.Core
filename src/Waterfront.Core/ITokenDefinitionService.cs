using System.Threading.Tasks;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens;

namespace Waterfront.Core;

public interface ITokenDefinitionService
{
    ValueTask<TokenDefinition> CreateTokenDefinitionAsync(
        TokenRequest request,
        TokenRequestAuthenticationResult authenticationResult,
        TokenRequestAuthorizationResult authorizationResult
    );
}