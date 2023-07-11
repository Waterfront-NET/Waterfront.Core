using DotNet.Globbing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.Common.Configuration;

namespace Waterfront.Core.Extensions.DependencyInjection;

internal class ServiceOptionsProvider<T> : IServiceOptionsProvider<T> where T : class, new()
{
    private static readonly GlobOptions s_GlobOptions = new GlobOptions {
        Evaluation = new EvaluationOptions { CaseInsensitive = true }
    };

    private readonly ILogger<ServiceOptionsProvider<T>> _logger;
    private readonly IReadOnlyDictionary<Glob, T>       _optionsMap;
    private readonly Dictionary<string, T>              _cachedMatchOptionsMap;

    public ServiceOptionsProvider(
        ILogger<ServiceOptionsProvider<T>> logger,
        IOptions<ServiceOptions<T>> options
    )
    {
        _logger = logger;
        _optionsMap = options.Value.Map.ToDictionary(
            k => Glob.Parse(k.Key, s_GlobOptions),
            v => v.Value
        );
        _cachedMatchOptionsMap = new Dictionary<string, T>();
    }

    public T Get(string service)
    {
        _logger.LogDebug("Finding options for service {Service}", service);

        if ( !_cachedMatchOptionsMap.TryGetValue(service, out T? options) )
        {
            _logger.LogDebug(
                "Cache miss: no previously matched options found for service {Service}",
                service
            );

            KeyValuePair<Glob, T> matchedPair = _optionsMap.First(x => x.Key.IsMatch(service));

            _logger.LogDebug(
                "Service {Service} was matched by pattern {Pattern}",
                service,
                matchedPair.Key.ToString()
            );

            _cachedMatchOptionsMap[service] = matchedPair.Value;
            options                         = matchedPair.Value;
        }

        return options;
    }
}
