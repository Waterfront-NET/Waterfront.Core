using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Waterfront.Acl.Static;
using Waterfront.Acl.Static.Models;
using Waterfront.Acl.Static.Options;
using Waterfront.AspNetCore.Extensions;
using Waterfront.Core.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (context, configuration) => configuration.MinimumLevel.Debug()
                                             .WriteTo.Console(theme: AnsiConsoleTheme.Code)
);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<StaticAclOptions>().Configure(
    options => {
        options.Users = new[] {
            new StaticAclUser {
                Username = "localhostUser",
                Ip = "127.0.0.1:*",
                Acl      = new[] { "default" }
            },
            new StaticAclUser {
                Username = "root",
                PlainTextPassword = "superuser",
                Acl = new[] {"admin"}
            }
        };

        options.Acl = new[] {
            new StaticAclPolicy {
                Name = "Default",
                Access = new[] {
                    new StaticAclPolicyAccessRule {
                        Name = "catalog",
                        Actions =
                        new[] { "pull", "push" },
                        Type = "repository"
                    }
                }
            }
        };
    });

builder.Services.AddWaterfront()
       .WithCertificateProvider<FileTokenCertificateProvider, FileTokenCertificateProviderOptions>(
           options => {
               options.CertificatePath = "./certs/localhost.crt";
               options.PrivateKeyPath  = "./certs/localhost.key";
           }
       )
       .WithAuthentication<StaticAclAuthenticationService>()
       .WithAuthorization<StaticAclAuthorizationService>();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseWaterfront(
    options => {
        options.TokenEndpoint = "/token";
    }
);

app.MapControllers();

app.Run();
