﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        WaterfrontBuilder builder = new WaterfrontBuilder(services);
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
        services.TryAddScoped<TokenMiddleware>();

        return services;
    }

    public static IApplicationBuilder UseWaterfront(
        this IApplicationBuilder builder,
        Action<EndpointOptions>? configureEndpoints = null
    )
    {
        ILogger<IApplicationBuilder> logger = builder.ApplicationServices
                                                     .GetRequiredService<
                                                         ILogger<IApplicationBuilder>>();

        IOptions<EndpointOptions> endpointOptions = builder.ApplicationServices
                                                           .GetRequiredService<
                                                               IOptions<EndpointOptions>>();

        if ( configureEndpoints != null )
            configureEndpoints(endpointOptions.Value);

        PathString tokenEndpoint = endpointOptions.Value.TokenEndpoint;

        if ( !tokenEndpoint.HasValue )
        {
            throw new InvalidOperationException(
                "Cannot use Waterfront without token endpoint configured"
            );
        }

        builder.Map(tokenEndpoint, app => app.UseMiddleware<TokenMiddleware>());
        logger.LogInformation("Token endpoint configured at {TokenEndpointPath}", tokenEndpoint);

        PathString infoEndpoint = endpointOptions.Value.InfoEndpoint; /*TODO*/

        if ( infoEndpoint.HasValue )
        {
            /*Register info endpoint*/
        }
        else
        {
            logger.LogWarning("No info endpoint configured");
        }

        PathString publicKeyEndpoint = endpointOptions.Value.PublicKeyEndpoint;

        if ( publicKeyEndpoint.HasValue )
        {
            /*Register public key endpoint*/
        }
        else
        {
            logger.LogWarning("No public key endpoint configured");
        }

        return builder;
    }
}
