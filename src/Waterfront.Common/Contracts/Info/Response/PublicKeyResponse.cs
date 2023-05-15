namespace Waterfront.Common.Contracts.Info.Response;

/// <summary>
/// Placeholder for later use
/// </summary>
public struct PublicKeyResponse
{
    /// <summary>
    /// Base-64 encoded public key
    /// </summary>
    public string Data { get; set; }
    /// <summary>
    /// Service, to which current public key is provided for
    /// </summary>
    public string Service { get; set; }

    public PublicKeyResponse()
    {
        throw new NotImplementedException();
    }
}
