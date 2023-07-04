using Waterfront.Common.Authentication;

namespace Waterfront.Core.Authentication;

public class AclAuthenticationSchemeBuilder
{
    private readonly List<string> _services;
    private readonly List<string> _clientIds;

    public string? Name { get; private set; }
    public string? DisplayName { get; private set; }
    public Type? HandlerType { get; private set; }
    public IReadOnlyCollection<string> Services => _services;
    public IReadOnlyCollection<string> ClientIds => _clientIds;

    public AclAuthenticationSchemeBuilder()
    {
        _services  = new List<string>();
        _clientIds = new List<string>();
    }

    public AclAuthenticationSchemeBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    public AclAuthenticationSchemeBuilder WithDisplayName(string displayName)
    {
        DisplayName = displayName;
        return this;
    }

    public AclAuthenticationSchemeBuilder WithHandler(Type handlerType)
    {
        if ( !handlerType.IsAssignableTo(typeof(IAclAuthenticationHandler)) )
        {
            throw new ArgumentException(
                "Invalid handler type: Handler type must implement " +
                nameof(IAclAuthenticationHandler),
                nameof(handlerType)
            );
        }

        HandlerType = handlerType;
        return this;
    }

    public AclAuthenticationSchemeBuilder WithHandler<T>() where T : IAclAuthenticationHandler =>
    WithHandler(typeof(T));

    public AclAuthenticationSchemeBuilder WithService(string service)
    {
        _services.Add(service);
        return this;
    }

    public AclAuthenticationSchemeBuilder WithServices(IEnumerable<string> services)
    {
        _services.AddRange(services);
        return this;
    }

    public AclAuthenticationSchemeBuilder WithServices(params string[] services) =>
    WithServices((IEnumerable<string>) services);

    public AclAuthenticationSchemeBuilder WithClientId(string clientId)
    {
        _clientIds.Add(clientId);
        return this;
    }

    public AclAuthenticationSchemeBuilder WithClientIds(IEnumerable<string> clientIds)
    {
        _clientIds.AddRange(clientIds);
        return this;
    }

    public AclAuthenticationSchemeBuilder WithClientIds(params string[] clientIds)
    {
        _clientIds.AddRange(clientIds);
        return this;
    }

    public AclAuthenticationScheme Build()
    {
        if ( string.IsNullOrEmpty(Name) )
        {
            throw new InvalidOperationException(
                nameof(AclAuthenticationScheme) + " should have a name"
            );
        }

        if ( HandlerType == null )
        {
            throw new InvalidOperationException(
                nameof(AclAuthenticationScheme) + " should have a handler type"
            );
        }

        return new AclAuthenticationScheme(
            Name,
            HandlerType,
            DisplayName,
            _services.Distinct(),
            _clientIds.Distinct()
        );
    }
}
