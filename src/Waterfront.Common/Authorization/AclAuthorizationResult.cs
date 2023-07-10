using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Common.Authorization;

public readonly struct AclAuthorizationResult
{
    public string Id { get; init; }
    public IReadOnlyList<TokenRequestScope> ForbiddenScopes { get; }
    public IReadOnlyList<TokenRequestScope> AuthorizedScopes { get; }
    public bool IsSuccessful => ForbiddenScopes.Count == 0;

    public AclAuthorizationResult(
        string id,
        IEnumerable<TokenRequestScope>? forbiddenScopes = null,
        IEnumerable<TokenRequestScope>? authorizedScopes = null
    )
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        Id = id;
        ForbiddenScopes = forbiddenScopes?.ToArray() ?? Array.Empty<TokenRequestScope>();
        AuthorizedScopes = authorizedScopes?.ToArray() ?? Array.Empty<TokenRequestScope>();
    }
}
