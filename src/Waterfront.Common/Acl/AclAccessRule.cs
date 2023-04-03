using System.Collections.Generic;

namespace Waterfront.Common.Acl;

public class AclAccessRule
{
    public AclResourceType Type { get; init; }
    public string Name { get; init; }
    public IEnumerable<AclResourceAction> Actions { get; init; }
}