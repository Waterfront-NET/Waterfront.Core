namespace Waterfront.Core.Utility.Parsing;

public static class BasicAuthParser
{
    public static (string username, string password) ParseAuthString(string? input)
    {
        if ( string.IsNullOrEmpty(input) )
        {
            return (string.Empty, string.Empty);
        }

        if ( !input.Contains(':') )
        {
            return (input, string.Empty);
        }

        var delimiterIndex = input.IndexOf(':');

        var username = input.Substring(0, delimiterIndex);

        if ( delimiterIndex == input.Length - 1 )
        {
            return (username, string.Empty);
        }

        var password = input.Substring(delimiterIndex + 1);

        return (username, password);
    }

    public static bool IsBasicAuth(string? headerValue)
    {
        if ( string.IsNullOrEmpty(headerValue) )
            return false;

        return headerValue.StartsWith("Basic ");
    }

    public static (string username, string password) ParseHeaderValue(string? input)
    {
        if ( !IsBasicAuth(input) )
        {
            return (string.Empty, string.Empty);
        }

        return ParseAuthString(input!.Substring(6));
    }
}
