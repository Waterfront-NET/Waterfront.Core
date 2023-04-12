using System;
using System.Text;
using System.Text.Json;
using JWT;
using Waterfront.Core.Json.Converters;

namespace Waterfront.Core.Tokens.Serializers;

public class TokenSerializer : IJsonSerializer
{
    public static readonly TokenSerializer Instance = new TokenSerializer();

    JsonSerializerOptions _options;

    private TokenSerializer()
    {
        _options = new JsonSerializerOptions
        {
            Converters = {
                TokenRequestScopeJsonConverter.Instance
            },
            WriteIndented = false
        };
    }

    public object Deserialize(Type type, string json)
    {
        return JsonSerializer.Deserialize(json, type, _options)!;
    }

    public string Serialize(object obj)
    {
        return JsonSerializer.Serialize(obj, _options);
    }
}
