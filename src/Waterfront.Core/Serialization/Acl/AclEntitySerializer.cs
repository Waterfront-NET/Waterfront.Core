using System;
using System.Linq;
using Waterfront.Common.Acl;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Core.Serialization.Acl;

public static class AclEntitySerializer
{
    public static string SerializeResourceType(AclResourceType resourceType) =>
    resourceType switch {
        AclResourceType.Repository => "repository",
        AclResourceType.Registry   => "registry",
        _                          => throw new ArgumentOutOfRangeException(nameof(resourceType))
    };

    public static string SerializeResourceAction(
        AclResourceAction resourceAction,
        bool anyAsWord = false
    ) =>
    resourceAction switch {
        AclResourceAction.Pull => "pull",
        AclResourceAction.Push => "push",
        AclResourceAction.Any  => anyAsWord ? "any" : "*",
        _                      => throw new ArgumentOutOfRangeException(nameof(resourceAction))
    };

    public static string SerializeTokenRequestScope(TokenRequestScope scope)
    {
        return string.Join(
            ":",
            scope.Type.ToSerialized(),
            scope.Name,
            string.Join(",", scope.Actions.Select(action => action.ToSerialized()))
        );
    }
}
