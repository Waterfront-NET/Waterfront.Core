namespace Waterfront.Common.Acl;

public class AclAccessRule
{
    public AclResourceType Type { get; }
    public string Name { get; }
    public IEnumerable<AclResourceAction> Actions { get; }
}