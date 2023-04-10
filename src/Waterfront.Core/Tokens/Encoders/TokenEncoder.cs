using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.Common.Tokens;
using Waterfront.Core.Configuration.Tokens;
using Waterfront.Core.Extensions.Cryptography;
using Waterfront.Core.Tokens.Signing.CertificateProviders;

namespace Waterfront.Core.Tokens.Encoders;

public class TokenEncoder : ITokenEncoder
{
    private readonly ILogger<TokenEncoder>       _logger;
    private readonly IOptions<TokenOptions>      _options;
    private readonly ISigningCertificateProvider _certificateProvider;

    public TokenEncoder(
        ILogger<TokenEncoder> logger,
        IOptions<TokenOptions> options,
        ISigningCertificateProvider certificateProvider
    )
    {
        _logger              = logger;
        _options             = options;
        _certificateProvider = certificateProvider;
    }

    public async ValueTask<string> EncodeTokenAsync(TokenDefinition definition)
    {
        DateTimeOffset iat = DateTimeOffset.UtcNow;
        DateTimeOffset eat = DateTimeOffset.UtcNow.Add(_options.Value.Lifetime);

        X509Certificate2 certificate = await _certificateProvider.GetCertificateAsync();

        JwtBuilder builder = JwtBuilder.Create()
                                       .WithAlgorithm(new RS256Algorithm(certificate))
                                       .Id(definition.Id)
                                       .Subject(definition.Subject)
                                       .Audience(definition.Service)
                                       .Issuer(_options.Value.Issuer)
                                       .IssuedAt(iat.ToUnixTimeSeconds())
                                       .ExpirationTime(eat.ToUnixTimeSeconds())
                                       .AddHeader(HeaderName.KeyId, certificate.KeyId())
                                       .AddClaim("access", definition.Access);

        return builder.Encode();
    }
}
