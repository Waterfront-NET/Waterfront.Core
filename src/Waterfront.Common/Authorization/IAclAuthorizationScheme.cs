namespace Waterfront.Common.Authorization;

public interface IAclAuthorizationScheme
{
    string Name { get; }

    ValueTask<AclAuthorizationResult> AuthorizeAsync();
}
