using System.Diagnostics;

namespace Waterfront.Common.Acl;

public enum AclResourceAction
{
    [DebuggerDisplay("pull")]
    Pull,
    [DebuggerDisplay("push")]
    Push,
    [DebuggerDisplay("*")]
    Any
}
