using System;
using System.Collections.Generic;

namespace Waterfront.Common.Tokens;

public readonly struct TokenDefinition
{
    public string Id { get; init; }
    public string Subject { get; init; }
    public string Service { get; init; }
    public string Issuer { get; init; }
    public DateTimeOffset IssuedAt { get; init; }
    public DateTimeOffset ExpiresAt { get; init; }
    public IEnumerable<TokenResponseAccessEntry> Access { get; init; }
}