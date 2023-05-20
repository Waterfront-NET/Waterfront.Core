using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Common.Authorization;

public readonly struct AclAuthorizationResult
{
    public bool IsSuccessful => !ForbiddenScopes.Any();
    public string Id { get; init; }
    public ICollection<TokenRequestScope> AuthorizedScopes { get; init; }
    public ICollection<TokenRequestScope> ForbiddenScopes { get; init; }

    public AclAuthorizationResult(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        Id = id;
        AuthorizedScopes = new List<TokenRequestScope>();
        ForbiddenScopes = new List<TokenRequestScope>();
    }

    public AclAuthorizationResult(TokenRequest request) : this(request.Id) { }
}
