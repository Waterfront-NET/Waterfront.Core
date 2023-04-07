using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.Acl.Static.Models;
using Waterfront.Acl.Static.Options;
using Waterfront.Common.Acl;
using Waterfront.Common.Authentication;
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

    public override ValueTask<AclAuthorizationResult> AuthorizeAsync(
        TokenRequest request,
        AclAuthenticationResult authnResult,
        AclAuthorizationResult currentResult
    )
    {
        Logger.LogTrace("AuthorizeAsync({RequestId})", request.Id);
        Logger.LogTrace("Authentication result: {@AuthenticationResult}", authnResult);

        if ( !authnResult.IsSuccessful )
        {
            Logger.LogError(
                "Cannot authorize request {RequestId}: Authentication result was not successful",
                request.Id
            );
            return ValueTask.FromResult(
                new AclAuthorizationResult { ForbiddenScopes = request.Scopes }
            );
        }

        var user = authnResult.User;

        List<TokenRequestScope> authorizedScopes = new List<TokenRequestScope>();
        List<TokenRequestScope> forbiddenScopes  = new List<TokenRequestScope>();

        var policies = Options.Value.Acl.Where(
                                  p => user.Acl.Contains(p.Name, StringComparer.OrdinalIgnoreCase)
                              )
                              .ToArray();

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
            new AclAuthorizationResult {
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
