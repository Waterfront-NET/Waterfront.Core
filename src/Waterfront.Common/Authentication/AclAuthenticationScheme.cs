namespace Waterfront.Common.Authentication;

public class AclAuthenticationScheme
{
    public string Name { get; }
    public string? DisplayName { get; }
    public Type HandlerType { get; }
    public string[] Services { get; }
    public string[] ClientIds { get; }

    public bool AllowsAnyService => Services.Length == 0;
    public bool RequireClientId => ClientIds.Length != 0;

    public AclAuthenticationScheme(
        string name,
        Type handlerType,
        string? displayName = null,
        IEnumerable<string>? services = null,
        IEnumerable<string>? clientIds = null
    )
    {
        Name = name;
        DisplayName = displayName;

        if (!handlerType.IsAssignableTo(typeof(IAclAuthenticationHandler)))
        {
            throw new Exception();
        }

        HandlerType = handlerType;

        Services        = services?.ToArray() ?? Array.Empty<string>();
        ClientIds       = clientIds?.ToArray() ?? Array.Empty<string>();
    }
}
