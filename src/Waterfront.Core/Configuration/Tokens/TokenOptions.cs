using System;

namespace Waterfront.Core.Configuration.Tokens;

public class TokenOptions
{
    /// <summary>
    /// Defines value written to an "iss" field in the token payload
    /// </summary>
    public string Issuer { get; set; } = "Waterfront";
    public TimeSpan Lifetime { get; set; } = TimeSpan.FromSeconds(60);
}
