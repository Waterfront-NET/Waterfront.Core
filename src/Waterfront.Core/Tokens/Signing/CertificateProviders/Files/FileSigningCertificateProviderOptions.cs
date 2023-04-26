#pragma warning disable CS8618

namespace Waterfront.Core.Tokens.Signing.CertificateProviders.Files;

public class FileSigningCertificateProviderOptions
{
    public string CertificatePath { get; set; }
    public string PrivateKeyPath { get; set; }

    internal string GetFullCertificatePath() => Path.GetFullPath(CertificatePath);
    internal string GetFullPrivateKeyPath() => Path.GetFullPath(PrivateKeyPath);
}
