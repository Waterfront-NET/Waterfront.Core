using System;
using System.Collections.Generic;
using System.Linq;
using Waterfront.Common.Acl;

namespace Waterfront.Common.Tokens.Requests;

public readonly struct TokenRequestScope : IEquatable<TokenRequestScope>
{
    public AclResourceType Type { get; init; }
    public string Name { get; init; }
    public IReadOnlyList<AclResourceAction> Actions { get; init; }

    public bool Equals(TokenRequestScope other) =>
    Type == other.Type && Name == other.Name && Actions.All(other.Actions.Contains);

    public override bool Equals(object? obj) => obj is TokenRequestScope other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(
        (int) Type,
        Name,
        Actions.Select(a => (int) a).Sum()
    );
}
