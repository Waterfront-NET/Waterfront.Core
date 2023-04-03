using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Waterfront.Core.Utility.Cryptography;

namespace Waterfront.Core.Extensions.Cryptography;

public static class LibTrustExtensions
{
    public static string KeyId(this PublicKey publicKey) =>
    LibTrustUtility.GetKeyId(publicKey.ExportSubjectPublicKeyInfo());

    public static string KeyId(this RSA rsaKey) =>
    LibTrustUtility.GetKeyId(rsaKey.ExportSubjectPublicKeyInfo());

    public static string KeyId(this X509Certificate2 x509Certificate2) =>
    x509Certificate2.PublicKey.KeyId();
}
