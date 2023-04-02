using System.Net;

namespace Waterfront.Common.Credentials;

public record ConnectionCredentials(IPAddress IP, int Port);