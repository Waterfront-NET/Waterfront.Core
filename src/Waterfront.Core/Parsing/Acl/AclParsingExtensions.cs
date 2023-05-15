using Waterfront.Common.Acl;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Core.Parsing.Acl;

public static class AclParsingExtensions
{
    public static AclResourceType ToResourceType(this string self)
    {
        return AclEntityParser.ParseResourceType(self);
    }

    public static AclResourceAction ToResourceAction(this string self)
    {
        return AclEntityParser.ParseResourceAction(self);
    }

    public static IEnumerable<AclResourceAction> ToResourceActionList(this string self)
    {
        return AclEntityParser.ParseResourceActionList(self);
    }

    public static TokenRequestScope ToTokenRequestScope(this string self)
    {
        return AclEntityParser.ParseTokenRequestScope(self);
    }
}
