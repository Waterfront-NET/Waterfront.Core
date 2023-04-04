using System.Threading.Tasks;
using Waterfront.Common.Tokens;

namespace Waterfront.Core.Jwt;

public interface ITokenEncoder
{
    ValueTask<string> EncodeTokenAsync(TokenDefinition tokenResponse);
}