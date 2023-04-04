using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        foreach ( TokenRequestScope scope in request.Scopes )
        {
            foreach ( StaticAclPolicy policy in policies )
            {
                if ( TryAuthorize(scope, policy) )
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

    private bool TryAuthorize(TokenRequestScope scope, StaticAclPolicy policy)
    {
        Logger.LogDebug("Trying to authorize scope {@Scope} with policy {@Policy}", scope, policy);

        var matchingByType =
        policy.Access.Where(rule => rule.Type.Equals(scope.Type.ToSerialized()));
        var matchingByname  = matchingByType.Where(rule => rule.Name.ToGlob().IsMatch(scope.Name));
        var matchingByCheck = matchingByname.Any(rule => CheckRequiredActions(rule, scope));

        Logger.LogDebug("MatchingByType: {@MatchingByType}", matchingByType);
        Logger.LogDebug("MatchingByName: {@MatchingByName}", matchingByname);
        Logger.LogDebug("Matching by check: {MatchingByCheck}", matchingByCheck);

        return matchingByCheck;
    }

    private bool CheckRequiredActions(StaticAclPolicyAccessRule rule, TokenRequestScope scope)
    {
        Logger.LogInformation("CheckRequiredActions({@Rule}, {@Scope})", rule, scope);

        bool containsAny = rule.Actions.Contains(AclResourceAction.Any.ToSerialized());

        Logger.LogInformation("ContainsAny: {ContainsAny}", containsAny);

        if ( containsAny )
        {
            return true;
        }

        bool containsAllRequired = scope.Actions.Select(AclEntitySerializer.ToSerialized)
                                        .All(rule.Actions.Contains);

        Logger.LogInformation("ContainsAllRequired: {ContainsAllRequired}", containsAllRequired);

        return containsAllRequired;
    }
}
