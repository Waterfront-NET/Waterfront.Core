using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Common.Authentication;

public interface IAclAuthenticationHandler
{
    Task InitializeAsync(AclAuthenticationScheme scheme);

    Task<AclAuthenticationResult> AuthenticateAsync(TokenRequest request);
}
