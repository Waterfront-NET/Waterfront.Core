using System;
using System.Collections.Generic;
using System.Linq;
using Waterfront.Common.Acl;

namespace Waterfront.Common.Tokens;

/// <summary>
/// 
/// </summary>
public readonly struct TokenRequestScope : IEquatable<TokenRequestScope>
{
    public AclResourceType Type { get; init; }
    public string Name { get; init; }
    public IEnumerable<AclResourceAction> Actions { get; init; }

    public static bool operator ==(TokenRequestScope first, TokenRequestScope second) =>
    first.Equals(second);

    public static bool operator !=(TokenRequestScope first, TokenRequestScope second) =>
    !first.Equals(second);

    public bool Equals(TokenRequestScope other) =>
    Type == other.Type && Name == other.Name && EquateActions(other.Actions);

    public override bool Equals(object? obj) => obj is TokenRequestScope other && Equals(other);

    public override int GetHashCode() => HashCode.Combine((int) Type, Name, Actions);

    private bool EquateActions(IEnumerable<AclResourceAction> other)
    {
        AclResourceAction[] array1 = Actions.ToArray();
        AclResourceAction[] array2 = other.ToArray();

        return array1.Length == array2.Length && array1.All(array2.Contains);
    }
}
