using Waterfront.Common.Acl;

namespace Waterfront.Common.Tokens.Requests;

public readonly struct TokenRequestScope : IEquatable<TokenRequestScope>
{
    public AclResourceType Type { get; init; }
    public string Name { get; init; }
    public IReadOnlyList<AclResourceAction> Actions { get; init; }

    public bool Equals(TokenRequestScope other) => GetHashCode().Equals(other.GetHashCode());

    public override bool Equals(object? obj) => obj is TokenRequestScope other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(
        (int)Type,
        Name,
        /*
         * we provide a sum here because it doesn't really matter the order
         * of actions, we just need to know if all of them are present
         */
        Actions.Select(a => (int)a).Sum()
    );

    public override string ToString()
    {
        return $"TokenRequestScope({Type:G}:{Name}:{string.Join(",", Actions.Select(action => action.ToString("G")))})";
    }
}
