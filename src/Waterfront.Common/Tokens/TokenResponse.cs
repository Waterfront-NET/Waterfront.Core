using System;
using System.Collections;
using System.Collections.Generic;

namespace Waterfront.Common.Tokens;

public class TokenResponse
{
    public string Id { get; init; }
    public string Subject { get; init; }
    public string Service { get; init; }
    public string Issuer { get; init; }
    public DateTimeOffset IssuedAt { get; init; }
    // public DateTimeOffset Expires
    public bool IsSuccessful { get; init; }
    public string? Token { get; init; }
    public IEnumerable<TokenResponseAccessEntry> Access { get; init; }
}
