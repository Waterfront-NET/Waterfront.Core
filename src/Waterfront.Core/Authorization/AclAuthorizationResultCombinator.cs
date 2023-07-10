using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Core.Authorization;

public class AclAuthorizationResultCombinator : IAclAuthorizationResultCombinator
{
    public virtual AclAuthorizationResult Combine(AclAuthorizationResult first, AclAuthorizationResult second)
    {
        if (first.Id != second.Id)
        {
            throw new InvalidOperationException(
                "Cannot combine " + nameof(AclAuthorizationResult) + "s with different Id"
            );
        }

        List<TokenRequestScope> authorizedScopes = new List<TokenRequestScope>(first.AuthorizedScopes);
        List<TokenRequestScope> forbiddenScopes = new List<TokenRequestScope>();

        foreach (TokenRequestScope scope in first.ForbiddenScopes)
        {
            if (second.AuthorizedScopes.Contains(scope))
            {
                authorizedScopes.Add(scope);
            }
            else
            {
                forbiddenScopes.Add(scope);
            }
        }

        return new AclAuthorizationResult(first.Id, forbiddenScopes, authorizedScopes);
    }
}
