using System;
using System.Threading.Tasks;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.Common.Tokens;
using Waterfront.Core.Configuration;
using Waterfront.Core.Configuration.Tokens;
using Waterfront.Core.Extensions.Cryptography;
using Waterfront.Core.Security.Cryptography;

namespace Waterfront.Core.Jwt;

public class TokenEncoder : ITokenEncoder
{
    private readonly ILogger<TokenEncoder> _logger;
    private readonly IOptions<TokenOptions>        _options;
    private readonly ITokenCertificateProvider     _certificateProvider;

    public TokenEncoder(
        ILogger<TokenEncoder> logger,
        IOptions<TokenOptions> options,
        ITokenCertificateProvider certificateProvider
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

        JwtBuilder builder = JwtBuilder.Create()
                                        .WithAlgorithm(
                                            new RS256Algorithm(
                                                await _certificateProvider.GetCertificateAsync()
                                            )
                                        )
                                        .Id(definition.Id)
                                        .Subject(definition.Subject)
                                        .Audience(definition.Service)
                                        .Issuer(_options.Value.Issuer)
                                        .IssuedAt(iat.ToUnixTimeSeconds())
                                        .ExpirationTime(eat.ToUnixTimeSeconds())
                                        .AddHeader(
                                            HeaderName.KeyId,
                                            (await _certificateProvider.GetPublicKeyAsync()).KeyId()
                                        )
                                        .AddClaim("access", definition.Access);

        return builder.Encode();
    }
}
