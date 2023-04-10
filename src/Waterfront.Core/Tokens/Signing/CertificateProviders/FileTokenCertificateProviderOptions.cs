using System.IO;

namespace Waterfront.Core.Tokens.Signing.CertificateProviders;

public class FileTokenCertificateProviderOptions
{
    public string CertificatePath { get; set; } = "certs/cert.pem";
    public string PrivateKeyPath { get; set; } = "certs/key.pem";

    internal string GetFullCertificatePath() => Path.GetFullPath(CertificatePath);
    internal string GetFullPrivateKeyPath() => Path.GetFullPath(PrivateKeyPath);
}
