namespace Waterfront.Common.Authentication;

public interface IAclAuthenticationHandlerProvider
{
    Task<IAclAuthenticationHandler> GetHandlerAsync(AclAuthenticationScheme scheme);
}
