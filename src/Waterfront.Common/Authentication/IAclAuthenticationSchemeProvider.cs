namespace Waterfront.Common.Authentication;

public interface IAclAuthenticationSchemeProvider
{
    IEnumerable<AclAuthenticationScheme> GetAllSchemes();

    bool HasScheme(string name);

    AclAuthenticationScheme? GetScheme(string scheme);

    bool AddScheme(AclAuthenticationScheme scheme);

    bool RemoveScheme(string name);
}
