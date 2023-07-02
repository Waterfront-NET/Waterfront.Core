namespace Waterfront.Common.Authentication;

public class AclAuthenticationScheme
{
    public string Name { get; }
    public string? DisplayName { get; }
    public Type HandlerType { get; }
    public string[]? Services { get; }
    public string[]? ClientIds { get; }
    public bool RequireClientId { get; }

    public AclAuthenticationScheme(
        string name,
        Type handlerType,
        string? displayName = null,
        IEnumerable<string>? services = null,
        IEnumerable<string>? clientIds = null,
        bool requireClientId = false
    )
    {
        Name = name;
        DisplayName = displayName;

        if (!handlerType.IsAssignableTo(typeof(IAclAuthenticationHandler)))
        {
            throw new Exception();
        }

        HandlerType = handlerType;

        Services = services?.ToArray();
        ClientIds = clientIds?.ToArray();
        RequireClientId = requireClientId;
    }
}
