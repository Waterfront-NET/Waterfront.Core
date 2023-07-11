using Microsoft.Extensions.DependencyInjection;

namespace Waterfront.Core.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IWaterfrontBuilder AddWaterfront(this IServiceCollection services)
    {
        return new WaterfrontBuilder(services);
    }
}
