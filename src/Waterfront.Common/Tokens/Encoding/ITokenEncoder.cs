using System.Threading.Tasks;
using Waterfront.Common.Tokens.Definition;

namespace Waterfront.Common.Tokens.Encoding;

public interface ITokenEncoder
{
    /// <summary>
    /// Created JWT string from given <see cref="TokenDefinition"/>
    /// </summary>
    /// <param name="definition">Token definition to be used as data source</param>
    /// <returns>JWT string of encoded token</returns>
    ValueTask<string> EncodeTokenAsync(TokenDefinition definition);
}
