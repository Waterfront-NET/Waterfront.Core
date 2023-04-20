using System;
using System.Collections.Generic;
using System.Text;
using Sprache;
using Waterfront.Common.Authentication.Credentials;

namespace Waterfront.Core.Parsing.Authentication;

public static class BasicAuthParser
{
    public static BasicCredentials ParseAuthString(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return new BasicCredentials {};
        }

        if (!input.Contains(':'))
        {
            return new BasicCredentials {Username = input};
        }

        int delimiterIndex = input.IndexOf(':');

        string username = input.Substring(0, delimiterIndex);

        if (delimiterIndex == input.Length - 1)
        {
            return new BasicCredentials {Username = username};
        }

        string password = input.Substring(delimiterIndex + 1);

        return new BasicCredentials {
            Username = username,
            Password = password
        };
    }

    public static bool IsBasicAuth(string? headerValue)
    {
        if (string.IsNullOrEmpty(headerValue))
            return false;

        return headerValue.StartsWith("Basic ");
    }

    public static BasicCredentials ParseHeaderValue(string? input)
    {
        if (!IsBasicAuth(input))
        {
            return new BasicCredentials {};
        }

        return ParseAuthString(Encoding.UTF8.GetString(Convert.FromBase64String(input![6..])));
    }

    private static class Grammar
    {
        public static readonly Parser<char> UsernamePasswordDelimiter = Parse.Char(':');

        public static readonly Parser<string> Username = Parse.AnyChar.Except(UsernamePasswordDelimiter).Many().Text();

        public static readonly Parser<string> Password = Parse.AnyChar.Many().Text();

        public static readonly Parser<IEnumerable<char>> BasicAuthorizationHeaderPrefix = Parse.String("Basic ");

        public static readonly Parser<string[]> UsernameAndPassword = Username
                                                                      .Then(
                                                                          username => UsernamePasswordDelimiter.Select(
                                                                              _ => username
                                                                          )
                                                                      )
                                                                      .Then(
                                                                          username => Password.Select(
                                                                              password => new[] {username, password}
                                                                          )
                                                                      );

        public static readonly Parser<string[]> Value = UsernameAndPassword
                                                        .Or(Username.Select(username => new[] {username}))
                                                        .Or(Parse.Return(Array.Empty<string>()).End());

        public static readonly Parser<string[]> EncodedValue = Parse.AnyChar.AtLeastOnce()
                                                                    .Text()
                                                                    .Select(
                                                                        x => Value.Parse(
                                                                            Encoding.UTF8.GetString(
                                                                                Convert.FromBase64String(x)
                                                                            )
                                                                        )
                                                                    );

        public static readonly Parser<string[]> HeaderValue = BasicAuthorizationHeaderPrefix.Then(_ => EncodedValue);
    }
}
