using System.Diagnostics;

namespace Waterfront.Common.Authorization;

[DebuggerDisplay("AclAuthorizationPolicy({Name}:{DisplayName})")]
public class AclAuthorizationPolicy
{
    public string Name { get; }
    public string? DisplayName { get; }

    public AclAuthorizationPolicy(string name, string? displayName = null)
    {
        Name        = name;
        DisplayName = displayName;
    }
}
