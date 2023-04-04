using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Waterfront.AspNetCore.Configuration;
using Waterfront.AspNetCore.Configuration.Endpoints;
using Waterfront.AspNetCore.Middleware;
using Waterfront.AspNetCore.Services.Authentication;
using Waterfront.AspNetCore.Services.Authorization;
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

    public static IServiceCollection AddWaterfront(
        this IServiceCollection services,
        Action<WaterfrontBuilder> configureWaterfront
    )
    {
        var builder = new WaterfrontBuilder(services);
        configureWaterfront(builder);
        return services;
    }

    public static IServiceCollection AddWaterfrontCore(this IServiceCollection services)
    {
        services.AddOptions();

        services.TryAddScoped<TokenRequestAuthenticationService>();
        services.TryAddScoped<TokenRequestAuthorizationService>();
        services.TryAddScoped<ITokenRequestService, TokenRequestService>();
        services.TryAddScoped<ITokenDefinitionService, TokenDefinitionService>();
        services.TryAddScoped<ITokenEncoder, TokenEncoder>();

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
