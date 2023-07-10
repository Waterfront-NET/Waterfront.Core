using Waterfront.Common.Authentication;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Common.Authorization;

public interface IAclAuthorizationHandler
{
    Task InitializeAsync(AclAuthorizationPolicy policy);

    Task<AclAuthorizationResult> AuthorizeAsync(TokenRequest request, AclAuthenticationResult authnResult);
}
