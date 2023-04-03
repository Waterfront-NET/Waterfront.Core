using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Waterfront.Core.Security.Cryptography;

public class FileTokenCertificateProvider
: TokenCertificateProvider<FileTokenCertificateProviderOptions>
{
    public FileTokenCertificateProvider(
        ILoggerFactory loggerFactory,
        IOptionsMonitor<FileTokenCertificateProviderOptions> optionsMonitor
    ) : base(loggerFactory, optionsMonitor) { }

    public override async ValueTask<X509Certificate2> GetCertificateAsync()
    {
        if (ShouldReload)
            await LoadAsync();

        return Certificate!;
    }

    private async ValueTask LoadAsync()
    {
        ValidateFilesExist();

        string certificateText = await File.ReadAllTextAsync(Options.CertificatePath);
        string keyText         = await File.ReadAllTextAsync(Options.PrivateKeyPath);

        Certificate = X509Certificate2.CreateFromPem(certificateText, keyText);
        OnCertificateLoaded();
    }

    private void ValidateFilesExist()
    {
        string certificatePath = Path.GetFullPath(Options.CertificatePath);

        if (!File.Exists(certificatePath))
        {
            Logger.LogError(
                "Could not find certificate at path {CertificatePath}",
                certificatePath
            );
            throw new FileNotFoundException("Could not find certificate file", certificatePath);
        }

        string privateKeyPath = Path.GetFullPath(Options.PrivateKeyPath);

        if (!File.Exists(privateKeyPath))
        {
            Logger.LogError("Could not find private key at path {PrivateKeyPath}", privateKeyPath);
            throw new FileNotFoundException("Could not find private key file", privateKeyPath);
        }
    }
}
