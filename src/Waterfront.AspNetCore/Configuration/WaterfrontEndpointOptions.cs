using Microsoft.AspNetCore.Http;

namespace Waterfront.AspNetCore.Configuration;

public class WaterfrontEndpointOptions
{
    public PathString TokenEndpoint { get; set; }
    public PathString InfoEndpoint { get; set; }
    public PathString PublicKeyEndpoint { get; set; }

    public bool IsTokenEndpointValid => !string.IsNullOrEmpty(TokenEndpoint);
    public bool IsInfoEndpointEnabled => !string.IsNullOrEmpty(InfoEndpoint);
    public bool IsPublicKeyEndpointEnabled => !string.IsNullOrEmpty(PublicKeyEndpoint);
}
