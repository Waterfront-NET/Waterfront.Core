using System.Threading.Tasks;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens;

namespace Waterfront.Core;

public interface ITokenDefinitionService
{
    ValueTask<TokenDefinition> CreateDefinitionAsync(
        TokenRequest request,
        AclAuthenticationResult authenticationResult,
        AclAuthorizationResult authorizationResult
    );
}
