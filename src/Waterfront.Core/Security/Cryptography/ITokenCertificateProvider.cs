﻿using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Waterfront.Core.Security.Cryptography;

public interface ITokenCertificateProvider
{
    /// <summary>
    /// Gets <see cref="X509Certificate2"/> to be used for token signing
    /// </summary>
    /// <returns>Signing certificate</returns>
    ValueTask<X509Certificate2> GetCertificateAsync();

    /// <summary>
    /// Gets <see cref="PublicKey"/> which can be used to verify tokens signed with certificate returned by <see cref="GetCertificateAsync"/>
    /// </summary>
    /// <returns>Public key of the signing certificate</returns>
    ValueTask<PublicKey> GetPublicKeyAsync();
}
