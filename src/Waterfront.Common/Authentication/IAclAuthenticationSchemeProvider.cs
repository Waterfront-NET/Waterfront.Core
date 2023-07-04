using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Common.Authentication;

public interface IAclAuthenticationSchemeProvider
{
    IReadOnlyCollection<AclAuthenticationScheme> Schemes { get; }
    IReadOnlyDictionary<string, AclAuthenticationScheme> SchemeMap { get; }

    Task<IEnumerable<AclAuthenticationScheme>> GetSchemesForRequestAsync(TokenRequest request);
    bool HasScheme(string name);
    AclAuthenticationScheme? GetScheme(string name);
}
