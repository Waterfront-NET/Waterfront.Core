using System.Collections.Generic;

namespace Waterfront.Common.Tokens;

public readonly struct TokenResponseAccessEntry
{
    public string Type { get; init; }
    public string Name { get; init; }
    public IEnumerable<string> Actions { get; init; }
}
