using System.Diagnostics.CodeAnalysis;
using Waterfront.Common.Acl;
using Waterfront.Common.Tokens;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Common.Authentication;

public readonly struct AclAuthenticationResult
{
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

    public static AclAuthenticationResult Failed(string id) =>
        new AclAuthenticationResult { Id = id };

    public static AclAuthenticationResult Failed(TokenRequest request) =>
        new AclAuthenticationResult { Id = request.Id };

    public override string ToString()
    {
        return $"AuthnResult{{Id({Id}) User({User?.Username})}}";
    }
}

