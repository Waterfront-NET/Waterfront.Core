using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Waterfront.Common.Tokens;

namespace Waterfront.Core;

public interface ITokenRequestCreationService
{
    ValueTask<TokenRequest> CreateRequestAsync(
        string service,
        IPAddress remoteIpAddress,
        int remotePort,
        string? account = null,
        string? clientId = null,
        bool offlineToken = false,
        IEnumerable<string>? scopes = null,
        string? basicAuthorization = null,
        string? refreshToken = null
    );
}