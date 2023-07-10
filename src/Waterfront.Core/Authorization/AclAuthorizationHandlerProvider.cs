using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Waterfront.Common.Authorization;

namespace Waterfront.Core.Authorization;

public class AclAuthorizationHandlerProvider : IAclAuthorizationHandlerProvider
{
    public ILogger<AclAuthorizationHandlerProvider> Logger { get; }
    public IServiceProvider ServiceProvider { get; }


    public AclAuthorizationHandlerProvider(
        ILogger<AclAuthorizationHandlerProvider> logger,
        IServiceProvider serviceProvider
    )
    {
        Logger = logger;
        ServiceProvider = serviceProvider;
    }

    public Task<IAclAuthorizationHandler> GetHandlerAsync(AclAuthorizationPolicy policy)
    {
        return Task.FromResult(
            (IAclAuthorizationHandler)ActivatorUtilities.CreateInstance(ServiceProvider, policy.HandlerType)
        );
    }
}
