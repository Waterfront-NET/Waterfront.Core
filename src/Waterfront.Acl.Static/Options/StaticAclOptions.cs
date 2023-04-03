using Waterfront.Acl.Static.Models;

namespace Waterfront.Acl.Static.Options;

public class StaticAclOptions
{
    public StaticAclUser[] Users { get; set; }
    public StaticAclPolicy[] Acl { get; set; }
}