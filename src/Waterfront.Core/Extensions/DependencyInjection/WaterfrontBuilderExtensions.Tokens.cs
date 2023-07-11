using Waterfront.Common.Tokens.Configuration;

namespace Waterfront.Core.Extensions.DependencyInjection;

public static partial class WaterfrontBuilderExtensions
{
    public static IWaterfrontBuilder ConfigureTokens(
        this IWaterfrontBuilder builder,
        Action<TokenOptions> configure
    )
    {
        return builder.ConfigureServiceOptions(configure);
    }

    public static IWaterfrontBuilder ConfigureTokens(
        this IWaterfrontBuilder builder,
        string service,
        Action<TokenOptions> configure
    )
    {
        return builder.ConfigureServiceOptions(service, configure);
    }

    public static IWaterfrontBuilder SetTokenLifetime(
        this IWaterfrontBuilder builder,
        string service,
        int lifetimeSeconds
    )
    {
        return builder.ConfigureTokens(service, opt => opt.SetLifetime(lifetimeSeconds));
    }

    public static IWaterfrontBuilder SetTokenLifetime(
        this IWaterfrontBuilder builder,
        int lifetimeSeconds
    )
    {
        return builder.ConfigureTokens(opt => opt.SetLifetime(lifetimeSeconds));
    }

    public static IWaterfrontBuilder SetTokenLifetime(
        this IWaterfrontBuilder builder,
        string service,
        TimeSpan lifetime
    )
    {
        return builder.ConfigureTokens(service, opt => opt.SetLifetime(lifetime));
    }

    public static IWaterfrontBuilder SetTokenLifetime(
        this IWaterfrontBuilder builder,
        TimeSpan lifetime
    )
    {
        return builder.ConfigureTokens(opt => opt.SetLifetime(lifetime));
    }

    public static IWaterfrontBuilder SetTokenIssuer(
        this IWaterfrontBuilder builder,
        string service,
        string issuer
    )
    {
        return builder.ConfigureTokens(service, opt => opt.SetIssuer(issuer));
    }

    public static IWaterfrontBuilder SetTokenIssuer(this IWaterfrontBuilder builder, string issuer)
    {
        return builder.ConfigureTokens(opt => opt.SetIssuer(issuer));
    }
}
