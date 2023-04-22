using FluentAssertions;
using Waterfront.Common.Authentication.Credentials;

namespace Waterfront.Common.Tests.Authentication.Credentials;

[TestClass]
public class RefreshTokenCredentialsTests
{
    [DataTestMethod]
    [DataRow("sometoken")]
    [DataRow("")]
    public void HasValueTest(string token)
    {
        var credentials = new RefreshTokenCredentials {Token = token};

        credentials.HasValue.Should().Be(!string.IsNullOrEmpty(token));
    }
}
