using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
using Waterfront.Common.Tokens.Definition;
using Waterfront.Common.Tokens.Signing.CertificateProviders;
using Waterfront.Core.Extensions.Cryptography;

namespace Waterfront.Core.Tokens.Signing.CertificateProviders;

/// <summary>
/// Implements basic caching for derived <see cref="ISigningCertificateProvider"/> based on <see cref="TokenDefinition.Service"/> value
/// </summary>
public abstract class SigningCertificateProviderBase : ISigningCertificateProvider
{
    private readonly Dictionary<string, X509Certificate2> _serviceCertificateMap;

    protected ILogger Logger { get; }

    protected SigningCertificateProviderBase(ILoggerFactory loggerFactory)
    {
        Logger = loggerFactory.CreateLogger(GetType());
        _serviceCertificateMap = new Dictionary<string, X509Certificate2>();
    }

    public virtual async ValueTask<X509Certificate2?> GetCertificateAsync(TokenDefinition tokenDefinition)
    {
        string service = tokenDefinition.Service;

        Logger.LogDebug("Retrieving certificate for token {Id}, Service: {Service}", tokenDefinition.Id, service);

        if (_serviceCertificateMap.TryGetValue(service, out X509Certificate2? certificate))
        {
            Logger.LogDebug("Certificate has been found in cache: {CertificateKeyId}", certificate.KeyId());
            return certificate;
        }

        certificate = await GetCertificateAsyncImpl(service);

        if (certificate != null)
        {
            Logger.LogDebug(
                "Certificate was retrieved from implementation method and saved: [{Service}]={KeyId}",
                service,
                certificate.KeyId()
            );
            _serviceCertificateMap[service] = certificate;
        }

        return certificate;
    }

    public virtual async ValueTask<PublicKey?> GetPublicKeyAsync(TokenDefinition tokenDefinition)
    {
        Logger.LogDebug(
            "Getting public key for token {Id}, Service: {Service}",
            tokenDefinition.Id,
            tokenDefinition.Service
        );
        X509Certificate2? certificate = await GetCertificateAsync(tokenDefinition);
        Logger.LogDebug("Certificate: {KeyId}", certificate?.KeyId());
        return certificate?.PublicKey;
    }

    protected abstract ValueTask<X509Certificate2?> GetCertificateAsyncImpl(string service);
}
