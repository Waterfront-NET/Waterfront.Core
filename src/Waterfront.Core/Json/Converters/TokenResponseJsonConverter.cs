using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Waterfront.Common.Contracts.Tokens.Response;

namespace Waterfront.Core.Json.Converters;

public class TokenResponseJsonConverter : JsonConverter<TokenResponse>
{
    public override TokenResponse Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        throw new NotSupportedException("Reading TokenResponse is not supported");
    }

    public override void Write(Utf8JsonWriter writer, TokenResponse value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("token", value.Token);
        writer.WriteString("access_token", value.AccessToken);
        if ( !string.IsNullOrEmpty(value.IssuedAt) )
        {
            writer.WriteString("issued_at", value.IssuedAt);
        }

        if ( value.ExpiresIn.HasValue )
        {
            writer.WriteNumber("expires_in", value.ExpiresIn.Value);
        }

        if ( !string.IsNullOrEmpty(value.RefreshToken) )
        {
            writer.WriteString("refresh_token", value.RefreshToken);
        }
        writer.WriteEndObject();
    }
}
