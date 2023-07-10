namespace Waterfront.Common.Authorization;

public interface IAclAuthorizationResultCombinator
{
    AclAuthorizationResult Combine(AclAuthorizationResult first, AclAuthorizationResult second);
}
