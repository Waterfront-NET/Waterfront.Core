using System.Threading.Tasks;
using Waterfront.Common.Tokens;

namespace Waterfront.Core.Jwt;

public interface ITokenCreationService
{
    ValueTask<string> CreateTokenAsync(TokenResponse tokenResponse);
}