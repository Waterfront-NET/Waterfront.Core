using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Common.Authorization;

public interface IAclAuthorizationPolicyProvider
{
    Task<IEnumerable<AclAuthorizationPolicy>> GetPoliciesAsync(TokenRequest request);
}
