using System;
using System.Text;
using System.Text.Json;
using JWT;
using Waterfront.Core.Json.Converters;

namespace Waterfront.Core.Tokens.Serializers;

public class TokenSerializer : IJsonSerializer
{
    JsonSerializerOptions _options;

    public TokenSerializer()
    {
        _options = new JsonSerializerOptions
        {
            Converters = {
                new TokenRequestScopeJsonConverter()
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
