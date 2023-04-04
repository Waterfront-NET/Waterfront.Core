using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.Common.Authentication;
using Waterfront.Common.Tokens;

namespace Waterfront.Core.Authentication;

public abstract class AclAuthenticationService<TOptions> : IAclAuthenticationService
where TOptions : class
{
    protected ILogger Logger { get; }
    protected IOptions<TOptions> Options { get; }

    protected AclAuthenticationService(ILoggerFactory loggerFactory, IOptions<TOptions> options)
    {
        Logger  = loggerFactory.CreateLogger(GetType());
        Options = options;
    }

    public abstract ValueTask<TokenRequestAuthenticationResult> AuthenticateAsync(
        TokenRequest request
    );
}
