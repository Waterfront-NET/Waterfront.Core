﻿using System;
using System.Text;

namespace Waterfront.Common.Authentication.Credentials;

public record BasicCredentials(string Username, string Password)
{
    public const  string HEADER_PREFIX        = "Basic ";
    private const int    HEADER_PREFIX_LENGTH = 6;

    public static readonly BasicCredentials
        Empty = new BasicCredentials(string.Empty, string.Empty);

    public bool IsEmpty => !HasUsername && !HasPassword;
    public bool HasUsername => !string.IsNullOrEmpty(Username);
    public bool HasPassword => !string.IsNullOrEmpty(Password);

    public static BasicCredentials Parse(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return Empty;
        }

        byte[] decodedBytes;

        if (input.StartsWith(HEADER_PREFIX))
        {
            decodedBytes = Convert.FromBase64String(input.Substring(HEADER_PREFIX_LENGTH));
        }
        else
        {
            decodedBytes = Convert.FromBase64String(input);
        }

        string decodedValue = Encoding.UTF8.GetString(decodedBytes);

        string[] parts = decodedValue.Split(':');

        return new BasicCredentials(parts[0], parts[1]);
    }

    /// <summary>
    /// Checks if header's value is Basic Authentication
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool CheckHeaderValue(string value)
    {
        return value.StartsWith(HEADER_PREFIX);
    }
}