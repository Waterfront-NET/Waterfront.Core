using System;
using System.Collections.Generic;
using System.Linq;
using Waterfront.Common.Authentication.Credentials;

namespace Waterfront.Common.Tokens;

/// <summary>
/// Describes the incoming request for JWT creation
/// </summary>
public readonly struct TokenRequest
{
    /// <summary>
    /// Request id. Should be unique
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Service, which requested token is provided for
    /// </summary>
    public string Service { get; }

    /// <summary>
    /// Optional account id
    /// </summary>
    public string? Account { get; }

    /// <summary>
    /// Optional client id
    /// </summary>
    public string? Client { get; }

    /// <summary>
    /// Optional indicator that client wants to acquire a refresh token
    /// to be used in subsequent requests to this authentication server,
    /// along with access token
    /// </summary>
    public bool OfflineToken { get; }

    /// <summary>
    /// Resource access that client wants to gain
    /// </summary>
    public IReadOnlyList<TokenRequestScope> Scopes { get; }
    /// <summary>
    /// Basic credentials provided with the request.
    /// Usually it's the value of the "Authorization" header
    /// </summary>
    public BasicCredentials BasicCredentials { get; }
    /// <summary>
    /// Connection credentials generated from the request context.
    /// </summary>
    public ConnectionCredentials ConnectionCredentials { get; }
    /// <summary>
    /// Refresh token credentials provided with the request
    /// </summary>
    public RefreshTokenCredentials RefreshTokenCredentials { get; }

    /// <summary>
    /// Checks if current request requires no authorization
    /// </summary>
    public bool IsAuthenticationRequest => !Scopes.Any();

    public TokenRequest(
        string id,
        string service,
        string? account,
        string? client,
        bool offlineToken,
        BasicCredentials basicCredentials,
        ConnectionCredentials connectionCredentials,
        RefreshTokenCredentials refreshTokenCredentials,
        IReadOnlyList<TokenRequestScope>? scopes
    )
    {
        if ( string.IsNullOrEmpty(id) )
        {
            throw new ArgumentNullException(nameof(id));
        }

        if ( string.IsNullOrEmpty(service) )
        {
            throw new ArgumentNullException(nameof(service));
        }

        Id               = id;
        Service          = service;
        Account          = account;
        Client           = client;
        OfflineToken     = offlineToken;
        Scopes           = scopes ?? Array.Empty<TokenRequestScope>();
        BasicCredentials = basicCredentials;
        ConnectionCredentials = connectionCredentials ??
                                throw new ArgumentNullException(nameof(connectionCredentials));
        RefreshTokenCredentials = refreshTokenCredentials;
    }
}
