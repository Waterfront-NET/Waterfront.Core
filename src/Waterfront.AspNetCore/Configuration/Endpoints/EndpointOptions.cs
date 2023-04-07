using Microsoft.AspNetCore.Http;

namespace Waterfront.AspNetCore.Configuration.Endpoints;

public class WaterfrontEndpointOptions
{
    public PathString TokenEndpoint { get; set; } = new PathString("/token");
    public PathString InfoEndpoint { get; set; } = new PathString("/info");
    public PathString PublicKeyEndpoint { get; set; } = new PathString("/public-key");
}
