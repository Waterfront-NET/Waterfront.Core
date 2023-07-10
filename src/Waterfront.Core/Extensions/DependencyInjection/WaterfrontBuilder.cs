using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Waterfront.Common.Tokens.Definition;
using Waterfront.Core.Configuration.Tokens;
using Waterfront.Core.Tokens.Definition;

namespace Waterfront.Core.Extensions.DependencyInjection;

internal class WaterfrontBuilder : IWaterfrontBuilder
{
    public IServiceCollection Services { get; }

    public WaterfrontBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IWaterfrontBuilder WithTokenOptionsProvider<T>(ServiceLifetime lifetim = ServiceLifetime.Singleton)
        where T : class, ITokenOptionsProvider
    {

    }

    public IWaterfrontBuilder WithTokenDefinitionService<T>(ServiceLifetime lifetime = ServiceLifetime.Singleton) where T : class, ITokenDefinitionService
    {
        var descriptor = ServiceDescriptor.Describe(typeof(ITokenDefinitionService), typeof(T), lifetime);
        Services.RemoveAll<ITokenDefinitionService>();
        Services.Add(descriptor);
        return this;
    }

    public IWaterfrontBuilder WithDefaultTokenDefinitionService()
    {
        return WithTokenDefinitionService<TokenDefinitionService>();
    }
}

public static class ServiceCollectionExtensions
{
    public static IWaterfrontBuilder AddWaterfront(this IServiceCollection services)
    {
        return new WaterfrontBuilder(services);
    }
}
