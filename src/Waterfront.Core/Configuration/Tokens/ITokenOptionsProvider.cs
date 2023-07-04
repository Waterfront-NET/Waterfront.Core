using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Core.Configuration.Tokens;

public interface ITokenOptionsProvider
{
    Task<TokenOptions> GetTokenOptionsAsync(TokenRequest request);
}
