using Waterfront.Common.Tokens.Definition;

namespace Waterfront.Core.Extensions.Tokens;

public static class TokenDefinitionExtensions
{
    public static int LifetimeSeconds(this TokenDefinition def) =>
    (int) (def.ExpiresAt - def.IssuedAt).TotalSeconds;
}
