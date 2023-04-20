using Waterfront.Common.Authentication.Credentials;

namespace Waterfront.Core.Parsing.Authentication;

public static class BasicAuthParserExtensions
{
    public static BasicCredentials ToBasicCredentials(this string self)
    {
        return BasicAuthParser.ParseAuthString(self);
    }
}
