using System.Security.Cryptography.X509Certificates;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Logging;
using Waterfront.Common.Tokens.Definition;
using Waterfront.Common.Tokens.Encoding;
using Waterfront.Common.Tokens.Signing.CertificateProviders;
using Waterfront.Core.Configuration.Tokens;
using Waterfront.Core.Extensions.Cryptography;
using Waterfront.Core.Serialization.Tokens;

namespace Waterfront.Core.Tokens.Encoding;

public class TokenEncoder : ITokenEncoder
{
    protected ILogger Logger { get; }
    protected ISigningCertificateProvider SigningCertificateProvider { get; }

    public TokenEncoder(
        ILoggerFactory loggerFactory,
        ISigningCertificateProvider signingCertificateProvider
    )
    {
        Logger                     = loggerFactory.CreateLogger(GetType());
        SigningCertificateProvider = signingCertificateProvider;
    }

    public virtual async ValueTask<string> EncodeTokenAsync(TokenDefinition definition)
    {
        Logger.LogDebug("Encoding token definition {Id}", definition.Id);

        X509Certificate2? certificate =
        await SigningCertificateProvider.GetCertificateAsync(definition);

        if ( certificate == null )
        {
            throw new Exception(
                "Could not retrieve certificate to sign token definition " + definition.Id
            );
        }

        return JwtBuilder.Create()
                         .WithAlgorithm(new RS256Algorithm(certificate))
                         .Id(definition.Id)
                         .Subject(definition.Subject)
                         .Audience(definition.Service)
                         .Issuer(definition.Issuer)
                         .IssuedAt(definition.IssuedAt.ToUnixTimeSeconds())
                         .ExpirationTime(definition.ExpiresAt.ToUnixTimeSeconds())
                         .AddHeader(HeaderName.KeyId, certificate.KeyId())
                         .AddClaim("access", definition.Access)
                         .WithJsonSerializer(TokenJsonSerializer.Instance)
                         .Encode();
    }
}
