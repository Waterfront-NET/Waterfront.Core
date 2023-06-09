﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Waterfront.Common.Acl;
using Waterfront.Common.Tokens.Requests;
using Waterfront.Core.Serialization.Acl;

namespace Waterfront.Core.Serialization.Tokens.Converters;

public class TokenRequestScopeJsonConverter : JsonConverter<TokenRequestScope>
{
    public static readonly TokenRequestScopeJsonConverter Instance =
        new TokenRequestScopeJsonConverter();

    public override TokenRequestScope Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    ) =>
        throw new NotSupportedException("Reading TokenRequestScope is not supported");

    public override void Write(
        Utf8JsonWriter writer,
        TokenRequestScope value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStartObject();
        writer.WriteString("type", value.Type.ToSerialized());
        writer.WriteString("name", value.Name);
        writer.WriteStartArray("actions");
        foreach (AclResourceAction action in value.Actions)
        {
            writer.WriteStringValue(action.ToSerialized());
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
    }
}
