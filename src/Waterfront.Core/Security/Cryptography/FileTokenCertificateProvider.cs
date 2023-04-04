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
        if ( ShouldReload )
            await LoadAsync();

        return Certificate!;
    }

    private async ValueTask LoadAsync()
    {
        ValidateFilesExist();

        string certificateText = await File.ReadAllTextAsync(Options.GetFullCertificatePath());
        string keyText         = await File.ReadAllTextAsync(Options.GetFullPrivateKeyPath());

        Certificate = X509Certificate2.CreateFromPem(certificateText, keyText);
        OnCertificateLoaded();
    }

    private void ValidateFilesExist()
    {
        if ( !File.Exists(Options.GetFullCertificatePath()) )
        {
            Logger.LogError(
                "Could not find certificate at path {CertificatePath}",
                Options.GetFullCertificatePath()
            );
            throw new FileNotFoundException(
                "Could not find certificate file",
                Options.GetFullCertificatePath()
            );
        }

        if ( !File.Exists(Options.GetFullPrivateKeyPath()) )
        {
            Logger.LogError(
                "Could not find private key at path {PrivateKeyPath}",
                Options.GetFullPrivateKeyPath()
            );
            throw new FileNotFoundException(
                "Could not find private key file",
                Options.GetFullPrivateKeyPath()
            );
        }
    }
}
