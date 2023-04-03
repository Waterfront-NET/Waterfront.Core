using System;
using Microsoft.Win32;
using Waterfront.Common.Acl;

namespace Waterfront.Core.Utility.Serialization.Acl;

public static class AclEntitySerializer
{
    public static string SerializeResourceType(AclResourceType resourceType) =>
    resourceType switch {
        AclResourceType.Repository => "repository",
        AclResourceType.Registry   => "registry",
        _                          => throw new ArgumentOutOfRangeException(nameof(resourceType))
    };

    public static string SerializeResourceAction(AclResourceAction resourceAction) =>
    resourceAction switch {
        AclResourceAction.Pull => "pull",
        AclResourceAction.Push => "push",
        AclResourceAction.Any  => "*",
        _                      => throw new ArgumentOutOfRangeException(nameof(resourceAction))
    };

    public static string ToSerialized(this AclResourceType self) => SerializeResourceType(self);

    public static string ToSerialized(this AclResourceAction self) => SerializeResourceAction(self);
}
