using Waterfront.Common.Acl;
using Waterfront.Core.Utility.Parsing.Acl;

namespace Waterfront.Acl.Static.Models;

public class StaticAclPolicyAccessRule
{
    public string Type { get; set; }
    public string Name { get; set; }
    public string[] Actions { get; set; }

    public AclAccessRule ToAclAccessRule()
    {
        return new AclAccessRule {
            Type = AclEntityParser.ParseResourceType(Type),
            Name = Name,
            Actions = Actions.Select(AclEntityParser.ParseResourceAction)
        };
    }

    public override string ToString()
    {
        return $"StaticACLPolicyAccessRule({Type}:{Name}:{string.Join(",", Actions)})";
    }
}