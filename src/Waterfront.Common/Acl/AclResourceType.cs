using System.Diagnostics;

namespace Waterfront.Common.Acl;

public enum AclResourceType
{
    [DebuggerDisplay("repository")]
    Repository,

    [DebuggerDisplay("registry")]
    Registry
}
