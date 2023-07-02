using System.Net;

namespace Waterfront.Common.Authentication.Credentials;

public readonly struct ConnectionCredentials : ITokenRequestCredentials
{
    public IPAddress IPAddress { get; init; }
    public int Port { get; init; }

    public bool HasValue => true;

    public override string ToString()
    {
        return ToString(true);
    }

    public string ToString(bool includePort)
    {
        string strIp = IPAddress.ToString();

        return includePort ? string.Concat(strIp, ":", Port.ToString()) : strIp;
    }
}
