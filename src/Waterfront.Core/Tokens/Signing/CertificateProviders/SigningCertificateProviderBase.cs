using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.Common.Tokens.Definition;
using Waterfront.Common.Tokens.Signing.CertificateProviders;

namespace Waterfront.Core.Tokens.Signing.CertificateProviders;

/// <summary>
/// Implements basic caching for derived <see cref="ISigningCertificateProvider"/>
/// </summary>
public abstract class SigningCertificateProviderBase : ISigningCertificateProvider
{
    protected ILogger Logger { get; }

    protected SigningCertificateProviderBase(ILoggerFactory loggerFactory)
    {
        Logger = loggerFactory.CreateLogger(GetType());
    }

    public abstract ValueTask<X509Certificate2?> GetCertificateAsync(
        TokenDefinition tokenDefinition
    );

    public virtual async ValueTask<PublicKey?> GetPublicKeyAsync(TokenDefinition tokenDefinition)
    {
        X509Certificate2? certificate = await GetCertificateAsync(tokenDefinition);
        return certificate?.PublicKey;
    }
}
