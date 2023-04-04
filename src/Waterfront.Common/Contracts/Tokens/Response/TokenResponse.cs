namespace Waterfront.Common.Contracts.Response;

public readonly struct TokenResponseDTO
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
