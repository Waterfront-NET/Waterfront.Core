using System.Collections;
using System.Collections.Generic;

namespace Waterfront.Common.Tokens;

public struct TokenResponseAccessEntry
{
    public string Type { get; set; }
    public string Name { get; set; }
    public IEnumerable<string> Actions { get; set; }
}
