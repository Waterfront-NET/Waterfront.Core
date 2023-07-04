#pragma warning disable CS8618

namespace Waterfront.Core.Tokens.Signing.CertificateProviders.Files;

public class FileSigningCertificateProviderOptions
{
    public string CertificatePath { get; set; }
    public string PrivateKeyPath { get; set; }
}
