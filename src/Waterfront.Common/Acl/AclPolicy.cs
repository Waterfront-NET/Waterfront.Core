namespace Waterfront.Common.Acl;

public class AclPolicy
{
    public string Name { get; }
    public IEnumerable<AclAccessRule> Access { get; }
}