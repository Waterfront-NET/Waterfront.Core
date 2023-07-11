using Waterfront.Common.Tokens.Configuration;

namespace Waterfront.Core.Extensions.DependencyInjection;

public static class TokenOptionsExtensions
{
    public static TokenOptions SetLifetime(this TokenOptions options, int lifetimeSeconds)
    {
        return options.SetLifetime(TimeSpan.FromSeconds(lifetimeSeconds));
    }

    public static TokenOptions SetLifetime(this TokenOptions options, TimeSpan lifetime)
    {
        options.Lifetime = lifetime;
        return options;
    }

    public static TokenOptions SetIssuer(this TokenOptions options, string issuer)
    {
        options.Issuer = issuer;
        return options;
    }
}
