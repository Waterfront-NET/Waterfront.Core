using System.Linq;
using System.Net;

namespace Waterfront.Common.Credentials;

public record ConnectionCredentials(IPAddress IP, int Port)
{
    public override string ToString()
    {
        return ToString(true);
    }

    public string ToString(bool includePort)
    {
        var strIp = IP.ToString();
        
        if (includePort)
        {
            return string.Concat(strIp, Port.ToString());
        }

        return strIp;
    }
}