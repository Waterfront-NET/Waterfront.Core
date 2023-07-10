using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Common.Authentication;

public interface IAclAuthenticationSchemeProvider
{
    IReadOnlyCollection<AclAuthenticationScheme> Schemes { get; }
    IReadOnlyDictionary<string, AclAuthenticationScheme> SchemeMap { get; }

    /// <summary>
    /// Resolves collection of <see cref="AclAuthenticationScheme"/> suitable for given <see cref="TokenRequest"/>
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IEnumerable<AclAuthenticationScheme>> GetSchemesAsync(TokenRequest request);

    bool HasScheme(string name);
    AclAuthenticationScheme? GetScheme(string name);
}
