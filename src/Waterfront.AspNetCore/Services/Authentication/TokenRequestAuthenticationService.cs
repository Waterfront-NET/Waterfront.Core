﻿using Microsoft.Extensions.Logging;
using Waterfront.Common.Authentication;
using Waterfront.Common.Tokens;
using Waterfront.Core.Authentication;

namespace Waterfront.AspNetCore.Services.Authentication;

public class TokenRequestAuthenticationService
{
    private readonly ILogger<TokenRequestAuthenticationService> _logger;
    private readonly IEnumerable<IAclAuthenticationService>     _authenticationServices;

    public TokenRequestAuthenticationService(
        ILogger<TokenRequestAuthenticationService> logger,
        IEnumerable<IAclAuthenticationService> authenticationServices
    )
    {
        _logger                 = logger;
        _authenticationServices = authenticationServices;
    }

    public async ValueTask<AclAuthenticationResult> AuthenticateAsync(TokenRequest request)
    {
        _logger.LogDebug("Authenticating TokenRequest {RequestId}", request.Id);
        
        AclAuthenticationResult result = AclAuthenticationResult.Failed;
        
        foreach (IAclAuthenticationService service in _authenticationServices)
        {
            AclAuthenticationResult currentResult = await service.AuthenticateAsync(request);

            if (currentResult.IsSuccessful)
            {
                result = currentResult;
                break;
            }
        }

        if (!result.IsSuccessful)
        {
            _logger.LogWarning("Failed to authenticate request {RequestId}", request.Id);
        }
        else
        {
            _logger.LogInformation(
                "Request {RequestId} was authenticated as user {Username}",
                request.Id,
                result.User!.Username
            );
        }
        
        return result;
    }
}
