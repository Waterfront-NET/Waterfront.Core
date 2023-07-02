using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.Common.Authentication;
using Waterfront.Common.Tokens.Requests;
using Waterfront.Core.Extensions.Enumerable;

namespace Waterfront.Core.Authentication;

public class AclAuthenticationSchemeProvider : IAclAuthenticationSchemeProvider
{
    private readonly AclAuthenticationOptions _options;

    public AclAuthenticationSchemeProvider(
        ILogger<AclAuthenticationSchemeProvider> logger,
        IOptions<AclAuthenticationOptions> options
    ) { }

    public IEnumerable<AclAuthenticationScheme> GetAllSchemes()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<AclAuthenticationScheme> GetSchemesForRequest(TokenRequest request) => _options.Schemes.Where(
        scheme => IsServiceAllowed(scheme, request.Service) && IsClientIdAllowed(scheme, request.ClientId)
    );

    public bool HasScheme(string name) => _options.SchemeMap.ContainsKey(name);

    public AclAuthenticationScheme? GetScheme(string name) => HasScheme(name) ? _options.SchemeMap[name] : null;

    public bool AddScheme(AclAuthenticationScheme scheme)
    {
        throw new NotImplementedException();
    }

    public bool RemoveScheme(string name)
    {
        throw new NotImplementedException();
    }

    private bool IsServiceAllowed(AclAuthenticationScheme scheme, string service) =>
        scheme.Services.IsNullOrEmpty() || scheme.Services.Contains(service);

    private bool IsClientIdAllowed(AclAuthenticationScheme scheme, string? clientId) => string.IsNullOrEmpty(clientId)
        ? !scheme.RequireClientId
        : scheme.ClientIds.IsNullOrEmpty() || scheme.ClientIds.Contains(clientId);
}
