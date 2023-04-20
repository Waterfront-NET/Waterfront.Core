namespace Waterfront.Common.Authentication.Credentials;

public readonly struct RefreshTokenCredentials
{
    public string Token { get; init; }

    public bool HasValue => !string.IsNullOrEmpty(Token);
}
