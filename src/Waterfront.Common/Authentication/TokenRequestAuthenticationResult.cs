using System.Diagnostics.CodeAnalysis;
using Waterfront.Common.Acl;
using Waterfront.Common.Tokens;

namespace Waterfront.Common.Authentication;

public readonly struct TokenRequestAuthenticationResult
{
    public static readonly TokenRequestAuthenticationResult Failed = default;

    [MemberNotNullWhen(true, "User")]
    public bool IsSuccessful => User != null;

    /// <summary>
    /// Id of the <see cref="TokenRequest" /> which this result is associated with 
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Subject of authentication
    /// </summary>
    public AclUser? User { get; init; }

    public static TokenRequestAuthenticationResult ForRequest(
        TokenRequest request,
        AclUser? user = null
    ) => new TokenRequestAuthenticationResult {
        Id   = request.Id,
        User = user
    };
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
