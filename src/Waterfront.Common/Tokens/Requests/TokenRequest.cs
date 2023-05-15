using Waterfront.Common.Authentication.Credentials;

namespace Waterfront.Common.Tokens.Requests;

/// <summary>
/// Describes the incoming request for JWT creation
/// </summary>
public readonly struct TokenRequest
{
    /// <summary>
    /// Request id. Should be unique
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Service, which requested token is provided for
    /// </summary>
    public string Service { get; init; }

    /// <summary>
    /// Optional account id
    /// </summary>
    public string? Account { get; init; }

    /// <summary>
    /// Optional client id
    /// </summary>
    public string? Client { get; init; }

    /// <summary>
    /// Optional indicator that client wants to acquire a refresh token
    /// to be used in subsequent requests to this authentication server,
    /// along with access token
    /// </summary>
    public bool OfflineToken { get; init; }

    /// <summary>
    /// Resource access that client wants to gain
    /// </summary>
    public ICollection<TokenRequestScope> Scopes { get; init; }

    /// <summary>
    /// Basic credentials provided with the request.
    /// Usually it's the value of the "Authorization" header
    /// </summary>
    public BasicCredentials BasicCredentials { get; init; }

    /// <summary>
    /// Connection credentials generated from the request context.
    /// </summary>
    public ConnectionCredentials ConnectionCredentials { get; init; }

    /// <summary>
    /// Refresh token credentials provided with the request
    /// </summary>
    public RefreshTokenCredentials RefreshTokenCredentials { get; init; }

    /// <summary>
    /// Checks if current request requires no authorization
    /// </summary>
    public bool IsAuthenticationRequest => !Scopes.Any();
}
