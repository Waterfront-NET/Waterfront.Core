using System;
using System.Collections.Generic;
using System.Text;
using Sprache;

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

        int delimiterIndex = input.IndexOf(':');

        string username = input.Substring(0, delimiterIndex);

        if ( delimiterIndex == input.Length - 1 )
        {
            return (username, string.Empty);
        }

        string password = input.Substring(delimiterIndex + 1);

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

        return ParseAuthString(Encoding.UTF8.GetString(Convert.FromBase64String(input![6..])));
    }

    private static class Grammar /*TODO: Complete besides the fact we need to convert value from base64 after parsing "Basic "*/
    {
        public static readonly Parser<char> UsernamePasswordDelimiter = Parse.Char(':');
        public static readonly Parser<string> Username = Parse.AnyChar.Except(UsernamePasswordDelimiter).Many().Text();
        public static readonly Parser<string> Password = Parse.AnyChar.Many().Text();

        public static readonly Parser<IEnumerable<char>> BasicAuthorizationHeaderPrefix =
        Parse.String("Basic ");

        public static readonly Parser<string[]> UsernameAndPassword = Username
        .Then(username => UsernamePasswordDelimiter.Select(_ => username))
        .Then(username => Password.Select(password => new[] { username, password }));

        public static readonly Parser<string[]> Value =
        UsernameAndPassword.Or(Username.Select(username => new[] { username })).Or(Parse.Return(Array.Empty<string>()).End());

        public static readonly Parser<string[]> HeaderValue =
        BasicAuthorizationHeaderPrefix.Then(_ => Value);
    }
}
