using System.Net;

namespace Waterfront.Common.Authentication.Credentials;

public record ConnectionCredentials(IPAddress IP, int Port)
{
    public override string ToString()
    {
        return ToString(true);
    }

    public string ToString(bool includePort)
    {
        string strIp = IP.ToString();

        return includePort ? string.Concat(strIp, ":", Port.ToString()) : strIp;
    }
}
