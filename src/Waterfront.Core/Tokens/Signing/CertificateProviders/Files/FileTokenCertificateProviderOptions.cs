using System.IO;

namespace Waterfront.Core.Tokens.Signing.CertificateProviders.Files;

public class FileTokenCertificateProviderOptions
{
    public string CertificatePath { get; set; }
    public string PrivateKeyPath { get; set; }

    internal string GetFullCertificatePath() => Path.GetFullPath(CertificatePath);
    internal string GetFullPrivateKeyPath() => Path.GetFullPath(PrivateKeyPath);
}
