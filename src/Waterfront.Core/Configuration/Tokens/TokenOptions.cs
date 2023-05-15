namespace Waterfront.Core.Configuration.Tokens;

public class TokenOptions
{
    /// <summary>
    /// Defines value written to an "iss" field in the token payload
    /// </summary>
    public string Issuer { get; set; } = "Waterfront";
    /// <summary>
    /// Defines relative lifetime of the issued token. Absolute lifetime is calculated as ExpiresAt = IssuedAt + Lifetime
    /// </summary>
    public TimeSpan Lifetime { get; set; } = TimeSpan.FromSeconds(60);
}
