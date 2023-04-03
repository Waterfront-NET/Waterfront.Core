using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Waterfront.Core.Security.Cryptography;

public interface ITokenCertificateProvider
{
    ValueTask<X509Certificate2> GetCertificateAsync();
    ValueTask<PublicKey> GetPublicKeyAsync();
}
