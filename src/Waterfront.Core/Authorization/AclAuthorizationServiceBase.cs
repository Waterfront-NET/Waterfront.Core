using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Core.Authorization;

public abstract class AclAuthorizationServiceBase<TOptions> : IAclAuthorizationService
where TOptions : class
{
    protected ILogger Logger { get; }
    protected IOptions<TOptions> Options { get; }

    protected AclAuthorizationServiceBase(ILoggerFactory loggerFactory, IOptions<TOptions> options)
    {
        Logger  = loggerFactory.CreateLogger(GetType());
        Options = options;
    }

    public abstract ValueTask<AclAuthorizationResult> AuthorizeAsync(
        TokenRequest request,
        AclAuthenticationResult authnResult,
        AclAuthorizationResult authzResult
    );
}
