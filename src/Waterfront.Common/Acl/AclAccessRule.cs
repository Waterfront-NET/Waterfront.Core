namespace Waterfront.Common.Acl;

#pragma warning disable CS8618

public class AclAccessRule
{
    public AclResourceType Type { get; init; }
    public string Name { get; init; }
    public IEnumerable<AclResourceAction> Actions { get; init; }
}