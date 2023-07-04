using System.Text.Json;
using JWT;
using Waterfront.Core.Serialization.Tokens.Converters;

namespace Waterfront.Core.Serialization.Tokens;

/// <summary>
/// Implements <see cref="IJsonSerializer"/> to use while encoding JWT
/// </summary>
public class TokenJsonSerializer : IJsonSerializer
{
    /// <summary>
    /// Shareable instance of <see cref="TokenJsonSerializer"/>
    /// Consider using this instead of creating new
    /// </summary>
    public static readonly TokenJsonSerializer Instance = new TokenJsonSerializer();

    private static readonly JsonSerializerOptions s_SerializerOptions = new JsonSerializerOptions {
        Converters    = { TokenRequestScopeJsonConverter.Instance },
        WriteIndented = false
    };

    public string Serialize(object obj) => JsonSerializer.Serialize(obj, s_SerializerOptions);

    public object Deserialize(Type type, string json) =>
    throw new NotSupportedException("Deserializing is not supported");
}
