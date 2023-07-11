using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
using Waterfront.Common.Configuration;
using Waterfront.Common.Tokens.Definition;
using Waterfront.Common.Tokens.Signing.CertificateProviders;

namespace Waterfront.Core.Tokens.Signing.CertificateProviders.File;

public class FileSigningCertificateProvider : ISigningCertificateProvider, IDisposable
{
    private readonly ILogger<FileSigningCertificateProvider>                _logger;
    private readonly IServiceOptionsProvider<FileSigningCertificateOptions> _optionsProvider;
    private readonly Dictionary<string, X509Certificate2>                   _certificates;

    private bool _disposed;

    public FileSigningCertificateProvider(
        ILogger<FileSigningCertificateProvider> logger,
        IServiceOptionsProvider<FileSigningCertificateOptions> optionsProvider
    )
    {
        _logger          = logger;
        _optionsProvider = optionsProvider;
        _certificates    = new();
    }

    public ValueTask<X509Certificate2?> GetCertificateAsync(TokenDefinition tokenDefinition)
    {
        if ( _disposed )
        {
            throw new ObjectDisposedException(nameof(FileSigningCertificateProvider));
        }

        string service = tokenDefinition.Service;

        _logger.LogDebug(
            "Getting certificate to sign token definition {Id} with service {Service}",
            tokenDefinition.Id,
            service
        );

        if ( !_certificates.TryGetValue(service, out var certificate) )
        {
            var options = _optionsProvider.Get(service);
            _logger.LogDebug("Cache miss, creating certificate from options: {@Options}", options);

            certificate = X509Certificate2.CreateFromPemFile(
                options.CertificatePath,
                options.PrivateKeyPath
            );
            _certificates[service] = certificate;
        }

        return ValueTask.FromResult(certificate)!;
    }

    public async ValueTask<PublicKey?> GetPublicKeyAsync(TokenDefinition tokenDefinition)
    {
        return (await GetCertificateAsync(tokenDefinition))!.PublicKey;
    }

    public void Dispose()
    {
        _certificates.Values.ToList().ForEach(x => x.Dispose());
        _certificates.Clear();
        _disposed = true;
    }
}
