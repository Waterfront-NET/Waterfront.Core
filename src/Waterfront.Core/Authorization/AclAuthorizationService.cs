using Microsoft.Extensions.Logging;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Core.Authorization;

public class AclAuthorizationService : IAclAuthorizationService
{
    protected ILogger Logger { get; }
    protected IAclAuthorizationPolicyProvider PolicyProvider { get; }
    protected IAclAuthorizationHandlerProvider HandlerProvider { get; }
    protected IAclAuthorizationResultCombinator ResultCombinator { get; }

    public AclAuthorizationService(
        ILoggerFactory loggerFactory,
        IAclAuthorizationPolicyProvider policyProvider,
        IAclAuthorizationHandlerProvider handlerProvider,
        IAclAuthorizationResultCombinator resultCombinator
    )
    {
        Logger = loggerFactory.CreateLogger(GetType());
        PolicyProvider = policyProvider;
        HandlerProvider = handlerProvider;
        ResultCombinator = resultCombinator;
    }


    public virtual async ValueTask<AclAuthorizationResult> AuthorizeAsync(
        TokenRequest request,
        AclAuthenticationResult authnResult
    )
    {
        if (request.IsAuthenticationRequest)
        {
            Logger.LogInformation(
                "Skipping authorization of request {Id}: no scopes requiring authorization found, request is an authentication request",
                request.Id
            );
            return new AclAuthorizationResult(request.Id);
        }

        IEnumerable<AclAuthorizationPolicy> policies = await PolicyProvider.GetPoliciesAsync(request);

        AclAuthorizationResult result = new AclAuthorizationResult(request.Id, request.Scopes);

        foreach (AclAuthorizationPolicy policy in policies)
        {
            if (result.IsSuccessful)
            {
                break;
            }

            IAclAuthorizationHandler? handler = await HandlerProvider.GetHandlerAsync(policy);

            if (handler == null)
            {
                Logger.LogWarning(
                    "Could not retrieve handler for policy {Policy} while processing request {Id}",
                    policy.Name,
                    request.Id
                );
                continue;
            }

            await handler.InitializeAsync(policy);

            Logger.LogDebug(
                "Initialized authz handler {HandlerType} with policy {PolicyName}",
                handler.GetType().Name,
                policy.Name
            );

            AclAuthorizationResult currentResult = await handler.AuthorizeAsync(request, authnResult);

            Logger.LogDebug("Got result from handler: {@Result}", currentResult);

            result = ResultCombinator.Combine(result, currentResult);

            Logger.LogDebug("Combined result: {@Result}", result);
        }

        Logger.LogInformation("Request {Id} result: {@Result}", request.Id, result);

        return result;
    }
}
