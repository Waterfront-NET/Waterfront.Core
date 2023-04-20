using System;
using System.Collections.Generic;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Common.Tokens.Definition;

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
    public ICollection<TokenRequestScope> Access { get; init; }

    public int LifetimeSeconds() => (int) (ExpiresAt - IssuedAt).TotalSeconds;
}
