using System.Security.Cryptography.X509Certificates;
using Waterfront.Common.Tokens.Definition;

namespace Waterfront.Common.Tokens.Signing.CertificateProviders;

public interface ISigningCertificateProvider
{
    /// <summary>
    /// Gets <see cref="X509Certificate2"/> to be used for token signing
    /// </summary>
    /// <returns>Signing certificate or null,
    /// if certificate for this <paramref name="tokenDefinition"/> could not be found
    /// </returns>
    ValueTask<X509Certificate2?> GetCertificateAsync(TokenDefinition tokenDefinition);

    /// <summary>
    /// Gets <see cref="PublicKey"/> which can be used to verify tokens signed with certificate returned by <see cref="GetCertificateAsync"/>
    /// </summary>
    /// <returns>Public key of the signing certificate or null,
    /// if certificate for this <paramref name="tokenDefinition"/> could not be found
    /// </returns>
    ValueTask<PublicKey?> GetPublicKeyAsync(TokenDefinition tokenDefinition);
}
