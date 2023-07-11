using Microsoft.Extensions.DependencyInjection;

namespace Waterfront.Core.Extensions.DependencyInjection;

public interface IWaterfrontBuilder
{
    IServiceCollection Services { get; }

    IWaterfrontBuilder AddServiceOptionsProvider<T>() where T : class, new();

    IWaterfrontBuilder ConfigureServiceOptions<T>(Action<ServiceOptions<T>> configure)
    where T : class, new();

    IWaterfrontBuilder ConfigureServiceOptions<T>(string servicePattern, Action<T> configure)
    where T : class, new();

    IWaterfrontBuilder ConfigureServiceOptions<T>(Action<T> configure) where T : class, new();
}
