using System.Text.Json;
using JWT;
using Waterfront.Core.Serialization.Tokens.Converters;

namespace Waterfront.Core.Serialization.Tokens;

public class TokenSerializer : IJsonSerializer
{
    public static readonly TokenSerializer Instance = new TokenSerializer();

    private readonly JsonSerializerOptions _options;

    private TokenSerializer()
    {
        _options = new JsonSerializerOptions {
            Converters = { TokenRequestScopeJsonConverter.Instance },
            WriteIndented = false
        };
    }

    public object Deserialize(Type type, string json) =>
        JsonSerializer.Deserialize(json, type, _options)!;

    public string Serialize(object obj) => JsonSerializer.Serialize(obj, _options);
}
