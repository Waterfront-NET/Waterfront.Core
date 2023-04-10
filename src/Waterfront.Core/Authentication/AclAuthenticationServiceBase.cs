using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.Common.Authentication;
using Waterfront.Common.Tokens;

namespace Waterfront.Core.Authentication;

public abstract class AclAuthenticationServiceBase<TOptions> : IAclAuthenticationService
where TOptions : class
{
    protected ILogger Logger { get; }
    protected IOptions<TOptions> Options { get; }

    protected AclAuthenticationServiceBase(ILoggerFactory loggerFactory, IOptions<TOptions> options)
    {
        Logger  = loggerFactory.CreateLogger(GetType());
        Options = options;
    }

    public abstract ValueTask<AclAuthenticationResult> AuthenticateAsync(
        TokenRequest request
    );
}
