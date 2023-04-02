using Waterfront.Common.Acl;

namespace Waterfront.Common.Authentication;

public class TokenRequestAuthenticationResult
{
    public bool IsSuccessful => User != null;
    public AclUser? User { get; }
}