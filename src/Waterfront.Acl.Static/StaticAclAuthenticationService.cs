using System.Linq;
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

    public override ValueTask<AclAuthenticationResult> AuthenticateAsync(
        TokenRequest request
    )
    {
        // FUck you
        Logger.LogDebug("Authorizing token request: {RequestId}", request.Id);

        if (TryAuthenticateWithBasicCredentials(
                request,
                out AclAuthenticationResult result1
            ))
        {
            Logger.LogDebug("Basic credentials matched");
            return ValueTask.FromResult(result1);
        }

        if (TryAuthenticateWithConnectionCredentials(
                request,
                out AclAuthenticationResult result2
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

        return ValueTask.FromResult(AclAuthenticationResult.Failed);
    }

    private bool TryAuthenticateWithConnectionCredentials(
        TokenRequest request,
        out AclAuthenticationResult result
    )
    {
        string matchTarget = request.ConnectionCredentials.ToString();

        StaticAclUser? user = Options.Value.Users.FirstOrDefault(
            user => !string.IsNullOrEmpty(user.Ip) && user.Ip.ToGlob().IsMatch(matchTarget)
        );

        result = new AclAuthenticationResult { User = user?.ToAclUser() };
        return result.IsSuccessful;
    }

    private bool TryAuthenticateWithBasicCredentials(
        TokenRequest request,
        out AclAuthenticationResult result
    )
    {
        if (request.BasicCredentials == null || request.BasicCredentials == BasicCredentials.Empty)
        {
            result = AclAuthenticationResult.Failed;
            return false;
        }

        StaticAclUser? user = Options.Value.Users.FirstOrDefault(
            user => user.Username.Equals(request.BasicCredentials.Username)
        );

        if (user == null)
        {
            result = AclAuthenticationResult.Failed;

            return false;
        }

        if (!string.IsNullOrEmpty(user.Password))
        {
            // This nesting is on purpose
            if (BCrypt.Net.BCrypt.Verify(request.BasicCredentials.Password, user.Password))
            {
                result = new AclAuthenticationResult { User = user.ToAclUser() };
                return true;
            }
        }
        else if (!string.IsNullOrEmpty(user.PlainTextPassword) &&
                 user.PlainTextPassword.Equals(request.BasicCredentials.Password))
        {
            result = new AclAuthenticationResult { User = user.ToAclUser() };
            return true;
        }

        result = AclAuthenticationResult.Failed;

        return false;
    }

    AclAuthenticationResult TryAuthenticateWithFallbackPolicy()
    {
        StaticAclUser? anonUser = Options.Value.Users.FirstOrDefault(
            user => string.IsNullOrEmpty(user.Password)          &&
                    string.IsNullOrEmpty(user.PlainTextPassword) &&
                    string.IsNullOrEmpty(user.Ip)
        );

        return new AclAuthenticationResult { User = anonUser?.ToAclUser() };
    }
}
