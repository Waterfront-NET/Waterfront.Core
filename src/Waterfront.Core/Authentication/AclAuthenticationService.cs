using Microsoft.Extensions.Logging;
using Waterfront.Common.Authentication;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Core.Authentication;

public class AclAuthenticationService : IAclAuthenticationService
{
    protected ILogger<AclAuthenticationService> Logger { get; }
    protected IAclAuthenticationSchemeProvider AuthenticationSchemeProvider { get; }
    protected IAclAuthenticationHandlerProvider AuthenticationHandlerProvider { get; }

    public AclAuthenticationService(
        ILogger<AclAuthenticationService> logger,
        IAclAuthenticationSchemeProvider authenticationSchemeProvider,
        IAclAuthenticationHandlerProvider handlerProvider
    )
    {
        Logger = logger;
        AuthenticationSchemeProvider = authenticationSchemeProvider;
        AuthenticationHandlerProvider = handlerProvider;
    }

    public async ValueTask<AclAuthenticationResult> AuthenticateAsync(TokenRequest request)
    {
        IEnumerable<AclAuthenticationScheme> availableSchemes =
            await AuthenticationSchemeProvider.GetSchemesForRequestAsync(request);

        foreach (AclAuthenticationScheme scheme in availableSchemes)
        {
            IAclAuthenticationHandler handler = await AuthenticationHandlerProvider.GetHandlerAsync(scheme);
            await handler.InitializeAsync(scheme);

            AclAuthenticationResult result = await handler.AuthenticateAsync(request);

            if (result.IsSuccessful)
            {
                return result;
            }
        }

        return AclAuthenticationResult.Fail(request.Id);
    }
}
