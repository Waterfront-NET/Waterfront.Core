﻿using Microsoft.Extensions.Logging;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Core.Authorization;

public abstract class AclAuthorizationServiceBase<TOptions> : IAclAuthorizationService
where TOptions : class
{
    protected ILogger Logger { get; }
    protected TOptions Options { get; }

    protected AclAuthorizationServiceBase(ILoggerFactory loggerFactory, TOptions options)
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
