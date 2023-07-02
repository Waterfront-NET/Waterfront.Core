using Waterfront.Common.Authentication;

namespace Waterfront.Core.Authentication;

public class AclAuthenticationOptions
{
    private readonly Dictionary<string, AclAuthenticationScheme> _schemes;

    public IReadOnlyCollection<AclAuthenticationScheme> Schemes => _schemes.Values;
    public IReadOnlyDictionary<string, AclAuthenticationScheme> SchemeMap => _schemes;

    public AclAuthenticationOptions()
    {
        _schemes = new Dictionary<string, AclAuthenticationScheme>();
    }

    public AclAuthenticationOptions AddScheme(Action<AclAuthenticationSchemeBuilder> buildScheme)
    {
        var builder = new AclAuthenticationSchemeBuilder();
        buildScheme(builder);
        return AddScheme(builder);
    }

    public AclAuthenticationOptions AddScheme(AclAuthenticationSchemeBuilder schemeBuilder)
    {
        return AddScheme(schemeBuilder.Build());
    }

    public AclAuthenticationOptions AddScheme(AclAuthenticationScheme scheme)
    {
        _schemes[scheme.Name] = scheme;
        return this;
    }

    public bool HasScheme(string name)
    {

    }

    public bool RemoveScheme(string name)
    {

    }
}
