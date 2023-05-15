using Microsoft.Extensions.Logging;
using Waterfront.Common.Authentication;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Core.Authentication;

public abstract class AclAuthenticationServiceBase<TOptions> : IAclAuthenticationService
where TOptions : class
{
    protected ILogger Logger { get; }
    protected TOptions Options { get; }

    protected AclAuthenticationServiceBase(ILoggerFactory loggerFactory, TOptions options)
    {
        Logger  = loggerFactory.CreateLogger(GetType());
        Options = options;
    }

    public abstract ValueTask<AclAuthenticationResult> AuthenticateAsync(
        TokenRequest request
    );
}
