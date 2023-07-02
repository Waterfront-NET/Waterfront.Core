using Waterfront.Common.Authentication;

namespace Waterfront.Core.Authentication;

public class AclAuthenticationSchemeBuilder
{
    private readonly HashSet<string> _services;
    private readonly HashSet<string> _clientIds;

    public string? Name { get; private set; }
    public string? DisplayName { get; private set; }
    public Type? HandlerType { get; private set; }
    public bool RequireClientId { get; private set; }
    public IReadOnlyCollection<string> Services => _services;
    public IReadOnlyCollection<string> ClientIds => _clientIds;

    public AclAuthenticationSchemeBuilder()
    {
        _services = new();
        _clientIds = new();
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
        HandlerType = handlerType;
        return this;
    }

    public AclAuthenticationSchemeBuilder WithHandler<T>()
    {
        return WithHandler(typeof(T));
    }

    public AclAuthenticationSchemeBuilder WithService(string service)
    {
        _ = _services.Add(service);
        return this;
    }

    public AclAuthenticationSchemeBuilder WithServices(IEnumerable<string> services)
    {
        foreach (string service in services)
        {
            _ = _services.Add(service);
        }

        return this;
    }

    public AclAuthenticationSchemeBuilder WithServices(params string[] services) =>
        WithServices((IEnumerable<string>)services);

    public AclAuthenticationSchemeBuilder WithClientId(string clientId)
    {
        _ = _clientIds.Add(clientId);
        return this;
    }

    public AclAuthenticationSchemeBuilder WithClientIds(IEnumerable<string> clientIds)
    {
        foreach (string clientId in clientIds)
        {
            _ = _clientIds.Add(clientId);
        }

        return this;
    }

    public AclAuthenticationSchemeBuilder WithClientIds(params string[] clientIds) =>
        WithClientIds((IEnumerable<string>)clientIds);

    public AclAuthenticationSchemeBuilder RequiresClientId()
    {
        RequireClientId = true;
        return this;
    }

    public AclAuthenticationScheme Build()
    {
        if (string.IsNullOrEmpty(Name))
        {
            throw new InvalidOperationException();
        }

        if (HandlerType == null)
        {
            throw new InvalidOperationException();
        }

        return new AclAuthenticationScheme(
            Name,
            HandlerType,
            DisplayName,
            _services is not {Count: 0} ? _services : null,
            _clientIds is not {Count: 0} ? _clientIds : null,
            RequireClientId
        );
    }
}
