using System.Collections.Generic;
using Waterfront.Common.Credentials;

namespace Waterfront.Common.Tokens;

public class TokenRequest
{
    public string Service { get; }
    public string? Account { get; }
    public string? Client { get; }
    public bool OfflineToken { get; }
    public IEnumerable<TokenRequestScope> Scopes { get; }
    public BasicCredentials? BasicCredentials { get; }
    public ConnectionCredentials? ConnectionCredentials { get; }
    public RefreshTokenCredentials? RefreshTokenCredentials { get; }
}