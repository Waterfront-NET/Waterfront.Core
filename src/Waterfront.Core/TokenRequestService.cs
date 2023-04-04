using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Waterfront.Common.Authentication.Credentials;
using Waterfront.Common.Tokens;
using Waterfront.Core.Utility.Parsing.Acl;

namespace Waterfront.Core;

public class TokenRequestService : ITokenRequestService
{
    private readonly ILogger<TokenRequestService> _logger;

    public TokenRequestService(ILogger<TokenRequestService> logger)
    {
        _logger = logger;
    }

    public ValueTask<TokenRequest> CreateRequestAsync(
        string service,
        IPAddress remoteIpAddress,
        int remotePort,
        string? account = null,
        string? clientId = null,
        bool offlineToken = false,
        IEnumerable<string>? scopes = null,
        string? basicAuthorization = null,
        string? refreshToken = null
    )
    {
        string requestId = Guid.NewGuid().ToString();

        _logger.LogDebug("Creating token request with ID {RequestId}", requestId);

        ConnectionCredentials connectionCredentials =
            new ConnectionCredentials(remoteIpAddress, remotePort);


        BasicCredentials basicCredentials = BasicCredentials.Parse(basicAuthorization);
        RefreshTokenCredentials refreshTokenCredentials =
        refreshToken != null
        ? new RefreshTokenCredentials(refreshToken)
        : RefreshTokenCredentials.Empty;

        _logger.LogDebug(
            "Connection credentials: {ConnectionCredentials}\n" +
            "Basic credentials: {BasicCredetnials}\n" +
            "Refresh token credentials: {RefreshTokenCredentials}",
            connectionCredentials,
            basicAuthorization,
            refreshTokenCredentials
        );
        IEnumerable<TokenRequestScope> requestScopes = scopes == null
            ? Array.Empty<TokenRequestScope>()
            : scopes.Select(AclEntityParser.ParseTokenRequestScope);

        TokenRequest tokenRequest = new TokenRequest(
            requestId,
            service,
            account,
            clientId,
            offlineToken,
            basicCredentials,
            connectionCredentials,
            refreshTokenCredentials,
            requestScopes
        );

        return ValueTask.FromResult(tokenRequest);
    }
}