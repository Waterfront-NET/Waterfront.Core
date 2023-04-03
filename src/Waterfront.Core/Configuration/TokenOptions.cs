using System;

namespace Waterfront.Core.Configuration;

public class TokenOptions
{
    public string Issuer { get; set; }
    public TimeSpan Lifetime { get; set; }
    
}
