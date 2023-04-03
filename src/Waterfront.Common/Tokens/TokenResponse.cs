using System;
using System.Collections.Generic;

namespace Waterfront.Common.Tokens;

public class TokenResponse
{
    public string Id { get; init; }
    public string Subject { get; init; }
    public string Service { get; init; }
    public string Issuer { get; init; }
    public DateTimeOffset IssuedAt { get; init; }
    public DateTimeOffset ExpiresAt { get; init; }
    public bool IsSuccessful { get; init; }
    public IEnumerable<TokenResponseAccessEntry> Access { get; init; }

    /// <summary>
    /// Gets the token lifetime
    /// </summary>
    /// <returns>Lifetime in whole seconds</returns>
    public int GetLifetime()
    {
        return (int)(ExpiresAt - IssuedAt).TotalSeconds;
    }
}