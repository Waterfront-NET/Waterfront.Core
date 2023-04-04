namespace Waterfront.Common.Contracts.Tokens.Response;

public readonly struct TokenResponse
{
    public string Token { get; init; }
    public string AccessToken => Token;
    /// <summary>
    /// Should be ISO-8601 DateTime string of token issued time. Optional
    /// </summary>
    public string? IssuedAt { get; init; }
    /// <summary>
    /// Should be in seconds. Optional
    /// </summary>
    public int? ExpiresIn { get; init; }
    /// <summary>
    /// Refresh token for subsequent requests. Optional
    /// </summary>
    public string? RefreshToken { get; init; }
}
