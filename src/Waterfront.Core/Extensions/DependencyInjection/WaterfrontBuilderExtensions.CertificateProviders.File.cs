using Microsoft.Extensions.DependencyInjection;
using Waterfront.Common.Tokens.Signing.CertificateProviders;
using Waterfront.Core.Tokens.Signing.CertificateProviders.File;

namespace Waterfront.Core.Extensions.DependencyInjection;

public static partial class WaterfrontBuilderExtensions
{
    public static IWaterfrontBuilder WithFileSigningCertificateProvider(
        this IWaterfrontBuilder builder
    )
    {
        builder.Services
               .AddSingleton<ISigningCertificateProvider, FileSigningCertificateProvider>();


    }


}
