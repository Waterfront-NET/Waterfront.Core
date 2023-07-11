using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Waterfront.Common.Configuration;
using Waterfront.Common.Tokens.Configuration;

namespace Waterfront.Core.Extensions.DependencyInjection;

internal class WaterfrontBuilder : IWaterfrontBuilder
{
    public IServiceCollection Services { get; }

    public WaterfrontBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IWaterfrontBuilder AddServiceOptionsProvider<T>() where T : class, new()
    {
        Services.AddSingleton<IServiceOptionsProvider<T>>();

        return this;
    }

    public IWaterfrontBuilder ConfigureServiceOptions<T>(Action<ServiceOptions<T>> configure)
    where T : class, new()
    {
        Services.AddSingleton<IConfigureOptions<ServiceOptions<T>>>(
            new ConfigureOptions<ServiceOptions<T>>(configure)
        );

        return this;
    }

    public IWaterfrontBuilder ConfigureServiceOptions<T>(string servicePattern, Action<T> configure)
    where T : class, new()
    {
        return ConfigureServiceOptions<T>(
            serviceOpt => serviceOpt.Configure(servicePattern, configure)
        );
    }

    public IWaterfrontBuilder ConfigureServiceOptions<T>(Action<T> configure) where T : class, new()
    {
        return ConfigureServiceOptions<T>(serviceOpt => serviceOpt.Configure(configure));
    }
}
