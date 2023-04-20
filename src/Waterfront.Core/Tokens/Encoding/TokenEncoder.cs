using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.Common.Tokens.Definition;
using Waterfront.Common.Tokens.Encoding;
using Waterfront.Common.Tokens.Signing.CertificateProviders;
using Waterfront.Core.Configuration.Tokens;
using Waterfront.Core.Extensions.Cryptography;
using Waterfront.Core.Serialization.Tokens;

namespace Waterfront.Core.Tokens.Encoding;

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
        _logger = logger;
        _options = options;
        _certificateProvider = certificateProvider;
    }

    public async ValueTask<string> EncodeTokenAsync(TokenDefinition definition)
    {
        _logger.LogDebug("Encoding token definition: {@Definition}", definition);

        DateTimeOffset iat = DateTimeOffset.UtcNow;
        DateTimeOffset exp = DateTimeOffset.UtcNow.Add(_options.Value.Lifetime);

        _logger.LogDebug("Issued at: {@Iat}\nExpires at: {@Exp}", iat, exp);

        X509Certificate2 certificate = await _certificateProvider.GetCertificateAsync();

        JwtBuilder builder = JwtBuilder.Create()
                                       .WithAlgorithm(new RS256Algorithm(certificate))
                                       .Id(definition.Id)
                                       .Subject(definition.Subject)
                                       .Audience(definition.Service)
                                       .Issuer(_options.Value.Issuer)
                                       .IssuedAt(iat.ToUnixTimeSeconds())
                                       .ExpirationTime(exp.ToUnixTimeSeconds())
                                       .AddHeader(HeaderName.KeyId, certificate.KeyId())
                                       .AddClaim("access", definition.Access)
                                       .WithJsonSerializer(TokenSerializer.Instance);

        return builder.Encode();
    }
}
