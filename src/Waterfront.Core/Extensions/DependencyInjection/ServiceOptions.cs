using System.Text.RegularExpressions;

namespace Waterfront.Core.Extensions.DependencyInjection;

public class ServiceOptions<T> where T : class, new()
{
    private const string DEFAULT_PATTERN = "**";

    private readonly Dictionary<string, T> _map;

    private T _defaultPatternOptions;

    public IReadOnlyDictionary<string, T> Map => _map;

    public ServiceOptions()
    {
        _map                   = new Dictionary<string, T>();
        _defaultPatternOptions = new T();
    }

    public ServiceOptions<T> Add(T options) => Add(DEFAULT_PATTERN, options);

    public ServiceOptions<T> Add(string pattern, T options)
    {
        if ( IsDefaultPattern(pattern) )
        {
            _defaultPatternOptions = options;
        }
        else
        {
            _map[pattern] = options;
        }

        return this;
    }

    public ServiceOptions<T> Configure(Action<T> configure) =>
    Configure(DEFAULT_PATTERN, configure);

    public ServiceOptions<T> Configure(string pattern, Action<T> configure)
    {
        T? value;

        if ( IsDefaultPattern(pattern) )
        {
            value = _defaultPatternOptions;
        }
        else if ( !_map.TryGetValue(pattern, out value) )
        {
            value         = new T();
            _map[pattern] = value;
        }

        configure(value);
        return this;
    }

    private bool IsDefaultPattern(string pattern) => Regex.IsMatch(pattern, @"^[*\\/]+$");
}
