namespace Waterfront.Common.Authentication.Credentials;

public record RefreshTokenCredentials(string Token)
{
    public static readonly RefreshTokenCredentials Empty = new RefreshTokenCredentials(string.Empty);
    public bool IsEmpty => string.IsNullOrEmpty(Token);
}