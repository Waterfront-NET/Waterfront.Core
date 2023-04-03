using System.Collections.Generic;
using Waterfront.Common.Acl;

namespace Waterfront.Common.Tokens;

public readonly struct TokenRequestScope
{
    public AclResourceType Type { get; init; }
    public string Name { get; init; }
    public IEnumerable<AclResourceAction> Actions { get; init; }
}
