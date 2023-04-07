﻿namespace Waterfront.Common.Authentication.Credentials;

public record BasicCredentials(string Username, string Password)
{
    public static readonly BasicCredentials
        Empty = new BasicCredentials(string.Empty, string.Empty);

    public bool IsEmpty => !HasUsername && !HasPassword;
    public bool HasUsername => !string.IsNullOrEmpty(Username);
    public bool HasPassword => !string.IsNullOrEmpty(Password);
}