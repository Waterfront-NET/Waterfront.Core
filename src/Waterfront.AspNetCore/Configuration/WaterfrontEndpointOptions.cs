namespace Waterfront.AspNetCore.Configuration;

public class WaterfrontEndpointOptions
{
    public string TokenEndpoint { get; set; }
    public string? InfoEndpoint { get; set; }
    public string? PublicKeyEndpoint { get; set; }

    public bool IsTokenEndpointValid => !string.IsNullOrEmpty(TokenEndpoint);
    public bool IsInfoEndpointEnabled => !string.IsNullOrEmpty(InfoEndpoint);
    public bool IsPublicKeyEndpointEnabled => !string.IsNullOrEmpty(PublicKeyEndpoint);
}
