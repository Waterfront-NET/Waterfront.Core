using System.Security.Principal;
using Waterfront.Common.Authentication;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Common.Authorization;

public interface IAclAuthorizationHandler
{
    ValueTask InitializeAsync(AclAuthorizationScheme authorizationScheme);

    ValueTask<AclAuthorizationResult> AuthorizeAsync(
        TokenRequest request,
        AclAuthenticationResult result
    );
}

public class AclAuthorizationScheme
{
    public string Name { get; }
    public string? DisplayName { get; }

    public string[] Services { get; }
    public string[] ClientIds { get; }

    public Type HandlerType { get; }

    public bool AllowsAnyService => Services.Length == 0;
    public bool RequiresClientId => ClientIds.Length != 0;

    public AclAuthorizationScheme(
        string name,
        string? displayName,
        IEnumerable<string> services,
        IEnumerable<string> clientIds,
        Type handlerType
    )
    {
        Name        = name;
        DisplayName = displayName;
        Services    = services.ToArray();
        ClientIds   = clientIds.ToArray();
        HandlerType = ValidateHandlerType(handlerType);
    }

    public AclAuthorizationScheme(string name, Type handlerType)
    {
        Name        = name;
        Services    = Array.Empty<string>();
        ClientIds   = Array.Empty<string>();
        HandlerType = ValidateHandlerType(handlerType);
    }

    private static Type ValidateHandlerType(Type handlerType)
    {
        return handlerType.IsAssignableTo(typeof(IAclAuthorizationHandler))
               ? handlerType
               : throw new ArgumentException(
                     "Invalid handler type: Handler type must implement " +
                     nameof(IAclAuthorizationHandler),
                     nameof(handlerType)
                 );
    }
}

public class AclAuthorizationSchemeBuilder
{
    private readonly List<string> _services;
    private readonly List<string> _clientIds;

    public string? Name { get; private set; }
    public string? DisplayName { get; private set; }

    public IReadOnlyList<string> Services => _services;
    public IReadOnlyList<string> ClientIds => _clientIds;



    public AclAuthorizationSchemeBuilder()
    {
        _services  = new List<string>();
        _clientIds = new List<string>();
    }

    public AclAuthorizationScheme Build()
    {
        throw new NotImplementedException();
    }
}
