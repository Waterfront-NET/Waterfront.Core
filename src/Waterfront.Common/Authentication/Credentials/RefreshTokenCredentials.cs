namespace Waterfront.Common.Authentication.Credentials;

public readonly struct RefreshTokenCredentials : ITokenRequestCredentials
{
    public string Token { get; init; }

    public bool HasValue => !string.IsNullOrEmpty(Token);
}
