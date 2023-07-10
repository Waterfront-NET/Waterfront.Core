using Waterfront.Common.Authorization;

namespace Waterfront.Core.Authorization;

public class AclAuthorizationPolicyBuilder
{
    public string? Name { get; private set; }
    public string? DisplayName { get; private set; }
    public Type? HandlerType { get; private set; }

    public static AclAuthorizationPolicyBuilder Create() => new();

    public AclAuthorizationPolicyBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    public AclAuthorizationPolicyBuilder WithDisplayName(string displayName)
    {
        DisplayName = displayName;
        return this;
    }

    public AclAuthorizationPolicyBuilder WithHandlerType(Type handlerType)
    {
        if (!handlerType.IsAssignableTo(typeof(IAclAuthorizationHandler)))
        {
            throw new ArgumentException(
                "Invalid handler type: handler must implement " + nameof(IAclAuthorizationHandler),
                nameof(handlerType)
            );
        }

        HandlerType = handlerType;
        return this;
    }

    public AclAuthorizationPolicyBuilder WithHandlerType<T>() where T : IAclAuthorizationHandler
    {
        return WithHandlerType(typeof(T));
    }

    public AclAuthorizationPolicy Build()
    {
        if (string.IsNullOrEmpty(Name))
        {
            throw new InvalidOperationException(nameof(AclAuthorizationPolicy) + " must have name");
        }

        if (HandlerType == null)
        {
            throw new InvalidOperationException(nameof(AclAuthorizationPolicy) + " must have handler type");
        }

        return new AclAuthorizationPolicy(Name, DisplayName, HandlerType);
    }
}
