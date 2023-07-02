using System.Diagnostics;

namespace Waterfront.Common.Acl;

#pragma warning disable CS8618

[DebuggerDisplay("{ToString()}")]
public class AclPolicy
{
    /// <summary>
    /// Policy's identifier. Should not be empty or duplicated, or unexpected behaviour is to be expected
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// A set of resource access rules which policy allows to match
    /// </summary>
    public IEnumerable<AclAccessRule> Access { get; init; }

    public override string ToString() =>
        $@"AclPolicy({Name}) [{string.Join(", ", Access.Select(rule => rule.ToString()))}]";
}
