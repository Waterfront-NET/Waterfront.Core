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
        Logger.LogDebug("Authenticating request {Id}", request.Id);

        Logger.LogDebug(
            "Has Basic credentials: {HasBasicCredentials}\nHas refresh token: {HasRefreshToken}\nHas connection credentials: {HasConnectionCredentials}",
            request.BasicCredentials.HasValue,
            request.RefreshTokenCredentials.HasValue,
            request.ConnectionCredentials.HasValue
        );

        IEnumerable<AclAuthenticationScheme> availableSchemes =
            (await AuthenticationSchemeProvider.GetSchemesAsync(request)).ToArray();

        Logger.LogDebug(
            "Authentication schemes: {AuthenticationSchemeList}",
            availableSchemes.Select(scheme => scheme.DisplayName ?? scheme.Name)
        );

        foreach (AclAuthenticationScheme scheme in availableSchemes)
        {
            IAclAuthenticationHandler handler = await AuthenticationHandlerProvider.GetHandlerAsync(scheme);
            await handler.InitializeAsync(scheme);

            Logger.LogDebug("Initialized handler {HandlerType}", handler.GetType().Name);

            AclAuthenticationResult result = await handler.AuthenticateAsync(request);

            Logger.LogDebug("Result: {IsResultSuccessful}", result.IsSuccessful);

            if (result.IsSuccessful)
            {
                Logger.LogDebug("User: {User}", result.User.Username);
                return result;
            }
        }

        return AclAuthenticationResult.Fail(request.Id);
    }
}
