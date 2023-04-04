using Waterfront.Common.Acl;

namespace Waterfront.Common.Authentication;

public readonly struct TokenRequestAuthenticationResult
{
    public static readonly TokenRequestAuthenticationResult Failed = default;

    public bool IsSuccessful => User != null;
    public AclUser? User { get; init; }
}

/*PROTOTYPE*/
/*public enum TokenRequestAuthenticationResultType
{
    Success,
    UserNotFound,
    InvalidPassword,
    InvalidRefreshToken,
    ExpiredRefreshToken
}*/
