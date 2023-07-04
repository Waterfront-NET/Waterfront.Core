using Waterfront.Common.Authentication;

namespace Waterfront.Core.Authentication;

public class AclAuthenticationOptions
{
    private readonly Dictionary<string, AclAuthenticationScheme> _schemes;

    /// <summary>
    /// All schemes
    /// </summary>
    public IReadOnlyCollection<AclAuthenticationScheme> Schemes => _schemes.Values;

    /// <summary>
    /// Mapping of scheme name to scheme
    /// </summary>
    public IReadOnlyDictionary<string, AclAuthenticationScheme> SchemeMap => _schemes;

    public AclAuthenticationOptions() =>
    _schemes = new Dictionary<string, AclAuthenticationScheme>();

    public AclAuthenticationOptions AddScheme(Action<AclAuthenticationSchemeBuilder> buildScheme)
    {
        AclAuthenticationSchemeBuilder builder = new AclAuthenticationSchemeBuilder();
        buildScheme(builder);
        return AddScheme(builder);
    }

    public AclAuthenticationOptions AddScheme(AclAuthenticationSchemeBuilder schemeBuilder) =>
    AddScheme(schemeBuilder.Build());

    public AclAuthenticationOptions AddScheme(AclAuthenticationScheme scheme)
    {
        _schemes[scheme.Name] = scheme;
        return this;
    }

    public AclAuthenticationOptions AddSchemes(
        params Action<AclAuthenticationSchemeBuilder>[] buildSchemes
    )
    {
        return AddSchemes(
            buildSchemes.Select(
                build => {
                    var builder = new AclAuthenticationSchemeBuilder();
                    build(builder);
                    return builder.Build();
                }
            )
        );
    }

    public AclAuthenticationOptions AddSchemes(IEnumerable<AclAuthenticationScheme> schemes)
    {
        foreach ( AclAuthenticationScheme scheme in schemes )
        {
            AddScheme(scheme);
        }

        return this;
    }

    public bool HasScheme(string name)
    {
        return _schemes.ContainsKey(name);
    }

    public bool RemoveScheme(string name)
    {
        return _schemes.Remove(name);
    }

    /// <summary>
    /// Removes schemes based on <paramref name="schemeFilter"/>'s return value
    /// </summary>
    /// <param name="schemeFilter"></param>
    /// <returns>Number of schemes removed</returns>
    public int RemoveSchemes(Predicate<AclAuthenticationScheme> schemeFilter)
    {
        int count = 0;
        foreach ( AclAuthenticationScheme scheme in Schemes.ToArray() )
        {
            bool shouldRemove = schemeFilter(scheme);

            if ( shouldRemove )
            {
                _schemes.Remove(scheme.Name);
                count++;
            }
        }

        return count;
    }
}
