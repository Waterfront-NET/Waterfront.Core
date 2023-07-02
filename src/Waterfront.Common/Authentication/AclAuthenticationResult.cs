using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Waterfront.Common.Acl;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Common.Authentication;

[DebuggerDisplay("{ToString()}")]
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

    public static AclAuthenticationResult Fail(string id) => new AclAuthenticationResult {Id = id};

    public static AclAuthenticationResult Success(string id, AclUser user) => new AclAuthenticationResult {
        Id = id,
        User = user
    };

    public override string ToString() => $"AclAuthenticationResult({Id}) {{ User({User}) }}";
}
