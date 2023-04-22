using System.Text;
using FluentAssertions;
using Waterfront.Common.Authentication.Credentials;

namespace Waterfront.Common.Tests.Authentication.Credentials;

[TestClass]
public class BasicCredentialsTests
{
    [DataTestMethod]
    [DataRow("anonymous", "")]
    [DataRow("not-anonymous", "mypwd")]
    [DataRow("", "")]
    [DataRow("", "mypwd")]
    public void HasValueTest(string username, string password)
    {
        BasicCredentials credentials = new BasicCredentials {
            Username = username,
            Password = password
        };

        credentials.HasValue.Should().Be(!string.IsNullOrEmpty(username));
    }

    [DataTestMethod]
    [DataRow("anonymous", "")]
    [DataRow("not-anonymous", "mypwd")]
    [DataRow("", "")]
    [DataRow("", "mypwd")]
    public void ToStringTest(string username, string password)
    {
        var credentials = new BasicCredentials {
            Username = username,
            Password = password
        };

        var plain = $"{username}:{password}";
        var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(plain));

        var plainOutput = credentials.ToString();
        var plainExplicitOutput = credentials.ToString(false);
        var base64Output = credentials.ToString(true);

        plainOutput.Should().Be(plain);
        plainExplicitOutput.Should().Be(plain);
        base64Output.Should().Be(base64);
    }
}
