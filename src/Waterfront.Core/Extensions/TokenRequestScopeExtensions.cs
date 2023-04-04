using System.Linq;
using Waterfront.Common.Tokens;
using Waterfront.Core.Utility.Serialization.Acl;

namespace Waterfront.Core.Extensions;

public static class TokenRequestScopeExtensions
{
    public static TokenResponseAccessEntry
    ToTokenResponseAccessEntry(this TokenRequestScope scope) => new TokenResponseAccessEntry {
        Type    = scope.Type.ToSerialized(),
        Name    = scope.Name,
        Actions = from action in scope.Actions select action.ToSerialized()
    };
}
