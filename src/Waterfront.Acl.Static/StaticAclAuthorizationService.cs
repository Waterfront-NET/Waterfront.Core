using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.Acl.Static.Models;
using Waterfront.Acl.Static.Options;
using Waterfront.Common.Acl;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens;
using Waterfront.Core;
using Waterfront.Core.Authorization;
using Waterfront.Core.Utility.Matching;
using Waterfront.Core.Utility.Serialization.Acl;

namespace Waterfront.Acl.Static;

public class StaticAclAuthorizationService : AclAuthorizationService<StaticAclOptions>
{
    public StaticAclAuthorizationService(
        ILoggerFactory loggerFactory,
        IOptions<StaticAclOptions> options
    ) : base(loggerFactory, options) { }

    public override ValueTask<TokenRequestAuthorizationResult> AuthorizeAsync(
        TokenRequest request,
        AclUser user
    )
    {
        Logger.LogInformation("Authorizing request {RequestId}", request.Id);

        List<TokenRequestScope> authorizedScopes = new List<TokenRequestScope>();
        List<TokenRequestScope> forbiddenScopes  = new List<TokenRequestScope>();

        var policies = Options.Value.Acl.Where(
            p => user.Acl.Contains(p.Name, StringComparer.OrdinalIgnoreCase)
        );

        foreach (TokenRequestScope scope in request.Scopes)
        {
            foreach (StaticAclPolicy policy in policies)
            {
                if (TryAuthorize(scope, policy))
                {
                    authorizedScopes.Add(scope);
                }
                else
                {
                    forbiddenScopes.Add(scope);
                }
            }
        }

        return ValueTask.FromResult(
            new TokenRequestAuthorizationResult {
                AuthorizedScopes = authorizedScopes,
                ForbiddenScopes  = forbiddenScopes
            }
        );
    }

    private static bool TryAuthorize(TokenRequestScope scope, StaticAclPolicy policy)
    {
        return policy.Access.Where(rule => rule.Type.Equals(scope.Type.ToSerialized()))
                     .Where(rule => rule.Name.ToGlob().IsMatch(scope.Name))
                     .Any(rule => CheckRequiredActions(rule, scope));
    }

    private static bool CheckRequiredActions(
        StaticAclPolicyAccessRule rule,
        TokenRequestScope scope
    ) =>
    rule.Actions.Contains(AclResourceAction.Any.ToSerialized()) ||
    scope.Actions.Select(AclEntitySerializer.ToSerialized).All(rule.Actions.Contains);
}
