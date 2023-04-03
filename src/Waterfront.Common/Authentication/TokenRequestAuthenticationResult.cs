using Waterfront.Common.Acl;

namespace Waterfront.Common.Authentication;

public class TokenRequestAuthenticationResult
{
    public static readonly TokenRequestAuthenticationResult Failed =
        new TokenRequestAuthenticationResult();

    public bool IsSuccessful => User != null;
    public AclUser? User { get; init; }
}