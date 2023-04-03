using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Waterfront.AspNetCore.Configuration;
using Waterfront.AspNetCore.Middleware;
using Waterfront.Core;
using Waterfront.Core.Configuration;
using Waterfront.Core.Jwt;

namespace Waterfront.AspNetCore.Extensions;

public static class WaterfrontExtensions
{
    public static WaterfrontBuilder AddWaterfront(this IServiceCollection services)
    {
        return new WaterfrontBuilder(services);
    }

    public static IServiceCollection AddWaterfrontCore(this IServiceCollection services)
    {
        services.AddOptions();
        
        services.TryAddScoped<ITokenRequestCreationService, TokenRequestCreationService>();
        services.TryAddScoped<ITokenResponseCreationService, TokenResponseCreationService>();
        services.TryAddScoped<ITokenCreationService, TokenCreationService>();

        return services;
    }

    public static IApplicationBuilder UseWaterfront(
        this IApplicationBuilder builder,
        Action<WaterfrontEndpointOptions>? configureEndpoints = null
    )
    {
        builder.UseMiddleware<WaterfrontMiddleware>();

        configureEndpoints?.Invoke(
            builder.ApplicationServices
                   .GetRequiredService<IOptions<WaterfrontEndpointOptions>>()
                   .Value
        );

        return builder;
    }
}
