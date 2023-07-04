using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Waterfront.Common.Contracts.Tokens.Response;

namespace Waterfront.Core.Serialization.Tokens.Converters;

public class TokenResponseJsonConverter : JsonConverter<TokenResponse>
{
    private const string PROPERTY_KEY_TOKEN         = "token";
    private const string PROPERTY_KEY_ACCESS_TOKEN  = "access_token";
    private const string PROPERTY_KEY_REFRESH_TOKEN = "refresh_token";
    private const string PROPERTY_KEY_IAT           = "issued_at";
    private const string PROPERTY_KEY_EXP           = "expires_in";

    /// <summary>
    /// Shareable instance of <see cref="TokenResponseJsonConverter"/>
    /// Consider using this instead of creating new
    /// </summary>
    public static readonly TokenResponseJsonConverter Instance = new TokenResponseJsonConverter();

    public override TokenResponse Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    ) => throw new NotSupportedException("Reading " + nameof(TokenResponse) + " is not supported");

    public override void Write(
        Utf8JsonWriter writer,
        TokenResponse value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStartObject();
        WriteTokens(writer, value);
        WriteMetadata(writer, value);
        writer.WriteEndObject();
    }

    private static void WriteTokens(Utf8JsonWriter writer, TokenResponse response)
    {
        writer.WriteString(PROPERTY_KEY_TOKEN, response.Token);
        writer.WriteString(PROPERTY_KEY_ACCESS_TOKEN, response.AccessToken);
        if ( !string.IsNullOrEmpty(response.RefreshToken) )
            writer.WriteString(PROPERTY_KEY_REFRESH_TOKEN, response.RefreshToken);
    }

    private static void WriteMetadata(Utf8JsonWriter writer, TokenResponse response)
    {
        if ( !string.IsNullOrEmpty(response.IssuedAt) )
            writer.WriteString(PROPERTY_KEY_IAT, response.IssuedAt);
        if ( response.ExpiresIn.HasValue )
            writer.WriteNumber(PROPERTY_KEY_EXP, response.ExpiresIn.Value);
    }
}
