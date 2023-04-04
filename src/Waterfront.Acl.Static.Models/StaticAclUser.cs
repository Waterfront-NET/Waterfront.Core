using Waterfront.Common.Acl;

namespace Waterfront.Acl.Static.Models;

#pragma warning disable CS8618

public class StaticAclUser
{
    public string Username { get; set; }
    public string? Ip { get; set; }
    public string? PlainTextPassword { get; set; }
    public string? Password { get; set; }
    public string[] Acl { get; set; }

    public AclUser ToAclUser()
    {
        return new AclUser {
            Username = Username,
            Acl = Acl
        };
    }

    public override string ToString()
    {
        return $"StaticACLUser({Username})";
    }
}