using System;
using System.Collections.Generic;
using System.Linq;
using Waterfront.Common.Credentials;

namespace Waterfront.Common.Tokens;

public readonly struct TokenRequest
{
    public string Id { get; }
    public string Service { get; }
    public string? Account { get; }
    public string? Client { get; }
    public bool OfflineToken { get; }
    public IEnumerable<TokenRequestScope> Scopes { get; }
    public BasicCredentials? BasicCredentials { get; }
    public ConnectionCredentials ConnectionCredentials { get; }
    public RefreshTokenCredentials? RefreshTokenCredentials { get; }

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
        IEnumerable<TokenRequestScope>? scopes,
        ConnectionCredentials connectionCredentials,
        BasicCredentials? basicCredentials,
        RefreshTokenCredentials? refreshTokenCredentials
    )
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (string.IsNullOrEmpty(service))
        {
            throw new ArgumentNullException(nameof(service));
        }

        Id = id;
        Service = service;
        Account = account;
        Client = client;
        OfflineToken = offlineToken;
        Scopes = scopes ?? Array.Empty<TokenRequestScope>();
        BasicCredentials = basicCredentials;
        ConnectionCredentials = connectionCredentials ?? throw new ArgumentNullException(nameof(connectionCredentials));
        RefreshTokenCredentials = refreshTokenCredentials;
    }
}