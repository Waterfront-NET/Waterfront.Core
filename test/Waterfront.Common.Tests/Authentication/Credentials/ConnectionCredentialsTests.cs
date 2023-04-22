using System.Net;
using FluentAssertions;
using Waterfront.Common.Authentication.Credentials;

namespace Waterfront.Common.Tests.Authentication.Credentials;

[TestClass]
public class ConnectionCredentialsTests
{
    private static IEnumerable<object[]> s_TestData => new object[][] {
        new object[] {
            new ConnectionCredentials {
                IPAddress = IPAddress.Parse("127.0.0.1"),
                Port = 28015
            }
        },
        new object[] {
            new ConnectionCredentials {
                IPAddress = IPAddress.Parse("192.168.0.1"),
                Port = 80
            }
        },
        new object[] {
            new ConnectionCredentials {
                IPAddress = IPAddress.Parse("8.8.8.8"),
                Port = 443
            }
        }
    };

    [DataTestMethod]
    [DynamicData(nameof(s_TestData))]
    public void ToStringTest(ConnectionCredentials subject)
    {
        var portOutput = subject.ToString();
        var portExplicitOutput = subject.ToString(true);
        var noPortOutput = subject.ToString(false);

        portOutput.Should().Be(portExplicitOutput).And.Be(subject.IPAddress + ":" + subject.Port);
        noPortOutput.Should().Be(subject.IPAddress.ToString());
    }
}
