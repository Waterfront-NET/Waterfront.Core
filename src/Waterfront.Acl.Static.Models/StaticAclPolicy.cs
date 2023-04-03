using Waterfront.Common.Acl;

namespace Waterfront.Acl.Static.Models;

public class StaticAclPolicy
{
    public string Name { get; set; }
    public StaticAclPolicyAccessRule[] Access { get; set; }

    public AclPolicy ToAclPolicy()
    {
        return new AclPolicy {
            Name = Name,
            Access = from rule in Access select rule.ToAclAccessRule()
        };
    }

    public override string ToString()
    {
        return $"StaticACLPolicy({Name})";
    }
}