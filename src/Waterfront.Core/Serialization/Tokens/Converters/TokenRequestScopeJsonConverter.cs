using System.Text.Json;
using System.Text.Json.Serialization;
using Waterfront.Common.Acl;
using Waterfront.Common.Tokens.Requests;
using Waterfront.Core.Serialization.Acl;

namespace Waterfront.Core.Serialization.Tokens.Converters;

public class TokenRequestScopeJsonConverter : JsonConverter<TokenRequestScope>
{
    private const string PROPERTY_KEY_TYPE    = "type";
    private const string PROPERTY_KEY_NAME    = "name";
    private const string PROPERTY_KEY_ACTIONS = "actions";

    /// <summary>
    /// Shareable instance of <see cref="TokenRequestScopeJsonConverter"/>
    /// Consider using this instead of creating new
    /// </summary>
    public static readonly TokenRequestScopeJsonConverter Instance =
    new TokenRequestScopeJsonConverter();

    public override TokenRequestScope Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    ) =>
    throw new NotSupportedException("Reading " + nameof(TokenRequestScope) + " is not supported");

    public override void Write(
        Utf8JsonWriter writer,
        TokenRequestScope value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStartObject();

        writer.WriteString(PROPERTY_KEY_TYPE, value.Type.ToSerialized());
        writer.WriteString(PROPERTY_KEY_NAME, value.Name);

        WriteActions(writer, value);

        writer.WriteEndObject();
    }

    private static void WriteActions(Utf8JsonWriter writer, TokenRequestScope scope)
    {
        writer.WriteStartArray(PROPERTY_KEY_ACTIONS);

        foreach ( string actionSerialized in from action in scope.Actions
                                             select action.ToSerialized() )
        {
            writer.WriteStringValue(actionSerialized);
        }

        writer.WriteEndArray();
    }
}
