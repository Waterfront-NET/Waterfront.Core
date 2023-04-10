using System;
using System.Collections.Generic;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;

namespace Waterfront.Common.Tokens;

/// <summary>
/// Provides a definition for the resulting JWT
/// </summary>
public readonly struct TokenDefinition
{
    public string Id { get; init; }
    public string Subject { get; init; }
    public string Service { get; init; }
    public string Issuer { get; init; }
    public DateTimeOffset IssuedAt { get; init; }
    public DateTimeOffset ExpiresAt { get; init; }
    public IReadOnlyList<TokenRequestScope> Access { get; init; }

    public int LifetimeSeconds() => (int) (ExpiresAt - IssuedAt).TotalSeconds;
}
