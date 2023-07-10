namespace Waterfront.Common.Authorization;

public interface IAclAuthorizationHandlerProvider
{
    Task<IAclAuthorizationHandler> GetHandlerAsync(AclAuthorizationPolicy policy);
}
