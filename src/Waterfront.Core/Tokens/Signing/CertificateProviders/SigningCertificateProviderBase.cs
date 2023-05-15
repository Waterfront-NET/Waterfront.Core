using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.Common.Tokens.Signing.CertificateProviders;

namespace Waterfront.Core.Tokens.Signing.CertificateProviders;

/// <summary>
/// Implements basic caching for derived <see cref="ISigningCertificateProvider"/>
/// </summary>
/// <typeparam name="TOptions"></typeparam>
public abstract class SigningCertificateProviderBase<TOptions> : ISigningCertificateProvider
where TOptions : class
{
    private readonly IOptionsMonitor<TOptions> _optionsMonitor;
    private          bool                      _optionsChanged;

    protected ILogger Logger { get; }
    protected TOptions Options => _optionsMonitor.CurrentValue;
    protected X509Certificate2? Certificate { get; set; }
    protected bool ShouldReload => Certificate == null || _optionsChanged;

    protected SigningCertificateProviderBase(
        ILoggerFactory loggerFactory,
        IOptionsMonitor<TOptions> optionsMonitor
    )
    {
        Logger          = loggerFactory.CreateLogger(GetType());
        _optionsMonitor = optionsMonitor;
        optionsMonitor.OnChange(_ => _optionsChanged = true);
    }

    public abstract ValueTask<X509Certificate2> GetCertificateAsync(string? service = null);

    public virtual async ValueTask<PublicKey> GetPublicKeyAsync(string? service = null)
    {
        X509Certificate2 certificate = await GetCertificateAsync(service);
        return certificate.PublicKey;
    }

    protected virtual void OnCertificateLoaded()
    {
        _optionsChanged = false;
    }
}
