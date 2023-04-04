﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.Acl.Static.Models;
using Waterfront.Acl.Static.Options;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authentication.Credentials;
using Waterfront.Common.Tokens;
using Waterfront.Core;
using Waterfront.Core.Authentication;
using Waterfront.Core.Utility.Matching;

namespace Waterfront.Acl.Static;

public class StaticAclAuthenticationService : AclAuthenticationService<StaticAclOptions>
{
    public StaticAclAuthenticationService(
        ILoggerFactory loggerFactory,
        IOptions<StaticAclOptions> options
    ) : base(loggerFactory, options) { }

    public override ValueTask<TokenRequestAuthenticationResult> AuthenticateAsync(
        TokenRequest request
    )
    {
        // FUck you
        Logger.LogDebug("Authorizing token request: {RequestId}", request.Id);

        if (TryAuthenticateWithBasicCredentials(
                request,
                out TokenRequestAuthenticationResult result1
            ))
        {
            Logger.LogDebug("Basic credentials matched");
            return ValueTask.FromResult(result1);
        }

        if (TryAuthenticateWithConnectionCredentials(
                request,
                out TokenRequestAuthenticationResult result2
            ))
        {
            Logger.LogDebug("Connection credentials matched");
            return ValueTask.FromResult(result2);
        }

        var result3 = TryAuthenticateWithFallbackPolicy();

        if (result3.IsSuccessful)
        {
            Logger.LogDebug("Found anonymous user");
            return ValueTask.FromResult(result3);
        }

        Logger.LogDebug("Auth failed");

        return ValueTask.FromResult(TokenRequestAuthenticationResult.Failed);
    }

    private bool TryAuthenticateWithConnectionCredentials(
        TokenRequest request,
        out TokenRequestAuthenticationResult result
    )
    {
        string matchTarget = request.ConnectionCredentials.ToString();

        StaticAclUser? user = Options.Value.Users.FirstOrDefault(
            user => !string.IsNullOrEmpty(user.Ip) && user.Ip.ToGlob().IsMatch(matchTarget)
        );

        result = new TokenRequestAuthenticationResult { User = user?.ToAclUser() };
        return result.IsSuccessful;
    }

    private bool TryAuthenticateWithBasicCredentials(
        TokenRequest request,
        out TokenRequestAuthenticationResult result
    )
    {
        if (request.BasicCredentials == null || request.BasicCredentials == BasicCredentials.Empty)
        {
            result = TokenRequestAuthenticationResult.Failed;
            return false;
        }

        StaticAclUser? user = Options.Value.Users.FirstOrDefault(
            user => user.Username.Equals(request.BasicCredentials.Username)
        );

        if (user == null)
        {
            result = TokenRequestAuthenticationResult.Failed;

            return false;
        }

        if (!string.IsNullOrEmpty(user.Password))
        {
            // This nesting is on purpose
            if (BCrypt.Net.BCrypt.Verify(request.BasicCredentials.Password, user.Password))
            {
                result = new TokenRequestAuthenticationResult { User = user.ToAclUser() };
                return true;
            }
        }
        else if (!string.IsNullOrEmpty(user.PlainTextPassword) &&
                 user.PlainTextPassword.Equals(request.BasicCredentials.Password))
        {
            result = new TokenRequestAuthenticationResult { User = user.ToAclUser() };
            return true;
        }

        result = TokenRequestAuthenticationResult.Failed;

        return false;
    }

    TokenRequestAuthenticationResult TryAuthenticateWithFallbackPolicy()
    {
        StaticAclUser? anonUser = Options.Value.Users.FirstOrDefault(
            user => string.IsNullOrEmpty(user.Password)          &&
                    string.IsNullOrEmpty(user.PlainTextPassword) &&
                    string.IsNullOrEmpty(user.Ip)
        );

        return new TokenRequestAuthenticationResult { User = anonUser?.ToAclUser() };
    }
}
