using Waterfront.Common.Tokens.Definition;

namespace Waterfront.Common.Contracts.Tokens.Response;

public readonly struct TokenResponse
{
    /// <summary>
    /// The JWT string
    /// </summary>
    public string Token { get; init; }

    /// <summary>
    /// Just a duplicate of the Token, to confine with oauth2 (referring to docker docs)
    /// </summary>
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

    public static TokenResponse Create(TokenDefinition definition, string token, string? refreshToken) =>
        new TokenResponse {
            Token = token,
            IssuedAt = definition.IssuedAt.UtcDateTime.ToString("O"),
            ExpiresIn = definition.LifetimeSeconds(),
            RefreshToken = refreshToken
        };
}
