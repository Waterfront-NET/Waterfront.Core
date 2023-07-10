namespace Waterfront.Common.Authorization;

public class AclAuthorizationPolicy
{
    public string Name { get; }
    public string? DisplayName { get; }
    public Type HandlerType { get; }

    public AclAuthorizationPolicy(string name, string? displayName, Type handlerType)
    {
        Name = name;
        DisplayName = displayName;
        HandlerType = handlerType.IsAssignableTo(typeof(IAclAuthorizationHandler))
            ? handlerType
            : throw new ArgumentException(
                "Invalid handler type: Handler must implement " + nameof(IAclAuthorizationHandler),
                nameof(handlerType)
            );
    }
}
