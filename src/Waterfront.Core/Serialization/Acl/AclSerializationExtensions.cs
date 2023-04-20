using Waterfront.Common.Acl;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Core.Serialization.Acl;

public static class AclSerializationExtensions
{
    public static string ToSerialized(this TokenRequestScope scope)
    {
        return AclEntitySerializer.SerializeTokenRequestScope(scope);
    }

    public static string ToSerialized(this AclResourceType self) =>
        AclEntitySerializer.SerializeResourceType(self);

    public static string ToSerialized(this AclResourceAction self, bool anyAsWord = false) =>
        AclEntitySerializer.SerializeResourceAction(self, anyAsWord);
}
