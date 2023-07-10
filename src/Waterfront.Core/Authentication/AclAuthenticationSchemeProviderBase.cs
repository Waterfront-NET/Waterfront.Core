using Waterfront.Common.Authentication;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Core.Authentication;

public abstract class AclAuthenticationSchemeProviderBase : IAclAuthenticationSchemeProvider
{
    public abstract IReadOnlyCollection<AclAuthenticationScheme> Schemes { get; }
    public abstract IReadOnlyDictionary<string, AclAuthenticationScheme> SchemeMap { get; }

    public virtual Task<IEnumerable<AclAuthenticationScheme>>
    GetSchemesAsync(TokenRequest request) => Task.FromResult(
        Schemes.Where(
            scheme => (scheme.AllowsAnyService || scheme.Services.Contains(request.Service)) &&
                      (!scheme.RequiresClientId ||
                       (request.ClientId != null && scheme.ClientIds.Contains(request.ClientId)))
        )
    );

    public virtual bool HasScheme(string name) => SchemeMap.ContainsKey(name);

    public virtual AclAuthenticationScheme? GetScheme(string name) =>
    HasScheme(name) ? SchemeMap[name] : null;
}
