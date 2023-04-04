using Waterfront.Acl.Static.Models;

namespace Waterfront.Acl.Static.Options;

#pragma warning disable CS8618

public class StaticAclOptions
{
    public StaticAclUser[] Users { get; set; }
    public StaticAclPolicy[] Acl { get; set; }

    public bool IsAuthenticationAvailable => Users is { Length: not 0 };
    public bool IsAuthorizationAvailable => Acl is { Length: not 0 };
}