using System.Security.Cryptography.X509Certificates;

namespace Waterfront.Common.Tokens.Signing.CertificateProviders;

public interface ISigningCertificateProvider
{
    /// <summary>
    /// Gets <see cref="X509Certificate2"/> to be used for token signing
    /// </summary>
    /// <returns>Signing certificate</returns>
    ValueTask<X509Certificate2> GetCertificateAsync(string? service = null);

    /// <summary>
    /// Gets <see cref="PublicKey"/> which can be used to verify tokens signed with certificate returned by <see cref="GetCertificateAsync"/>
    /// </summary>
    /// <returns>Public key of the signing certificate</returns>
    ValueTask<PublicKey> GetPublicKeyAsync(string? service = null);
}
