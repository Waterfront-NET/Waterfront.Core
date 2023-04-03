using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Waterfront.Core.Security.Cryptography;

public abstract class TokenCertificateProvider<TOptions> : ITokenCertificateProvider
where TOptions : class
{
    private readonly IOptionsMonitor<TOptions> _optionsMonitor;
    private          bool                      _optionsChanged;

    protected ILogger Logger { get; }
    protected TOptions Options => _optionsMonitor.CurrentValue;
    protected X509Certificate2? Certificate { get; set; }
    protected bool ShouldReload => Certificate == null || _optionsChanged;

    protected TokenCertificateProvider(
        ILoggerFactory loggerFactory,
        IOptionsMonitor<TOptions> optionsMonitor
    )
    {
        Logger          = loggerFactory.CreateLogger(GetType());
        _optionsMonitor = optionsMonitor;
        optionsMonitor.OnChange(_ => _optionsChanged = true);
    }

    public abstract ValueTask<X509Certificate2> GetCertificateAsync();

    public virtual async ValueTask<PublicKey> GetPublicKeyAsync()
    {
        X509Certificate2 certificate = await GetCertificateAsync();
        return certificate.PublicKey;
    }

    protected virtual void OnCertificateLoaded()
    {
        _optionsChanged = false;
    }
}
