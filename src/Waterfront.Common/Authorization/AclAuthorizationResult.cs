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
        Id = id;
        AuthorizedScopes = new List<TokenRequestScope>();
        ForbiddenScopes = new List<TokenRequestScope>();
    }

    public AclAuthorizationResult() : this(string.Empty) { }

    public AclAuthorizationResult(TokenRequest request) : this(request.Id) { }
}
