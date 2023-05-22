using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using Waterfront.Common.Acl;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Common.Authentication;

public readonly struct AclAuthenticationResult
{
    [MemberNotNullWhen(true, nameof(User))]
    public bool IsSuccessful => User != null;

    /// <summary>
    /// Id of the <see cref="TokenRequest" /> which this result is associated with
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Subject of authentication
    /// </summary>
    public AclUser? User { get; init; }

    [Obsolete("Use AclAuthenticationResult.Fail(string) instead")]
    public static AclAuthenticationResult Failed(string id) => Fail(id);

    public static AclAuthenticationResult Fail(string id) => new AclAuthenticationResult {Id = id};

    [Obsolete("Use AclAuthenticationResult.Fail(TokenRequest) instead")]
    public static AclAuthenticationResult Failed(TokenRequest request) => Fail(request);

    public static AclAuthenticationResult Fail(TokenRequest request) => Fail(request.Id);

    public static AclAuthenticationResult Success(string id, AclUser user) => new AclAuthenticationResult {
        Id = id,
        User = user
    };

    public static AclAuthenticationResult Success(TokenRequest request, AclUser user) => Success(request.Id, user);

    public override string ToString() => $"AuthnResult{{Id({Id}) User({User?.Username})}}";
}
