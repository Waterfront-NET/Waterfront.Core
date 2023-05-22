using System.Text;

namespace Waterfront.Common.Authentication.Credentials;

public readonly struct BasicCredentials
{
    public string Username { get; init; }
    public string Password { get; init; }

    public bool HasValue => !string.IsNullOrEmpty(Username);

    public override string ToString()
    {
        return ToString(false);
    }

    public string ToString(bool base64)
    {
        var strCreds = $"{Username}:{Password}";

        if (base64)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(strCreds));
        }

        return strCreds;
    }
}
