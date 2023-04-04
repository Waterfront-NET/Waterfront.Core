using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Waterfront.Acl.Static;
using Waterfront.Acl.Static.Models;
using Waterfront.Acl.Static.Options;
using Waterfront.AspNetCore.Extensions;
using Waterfront.Core.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables("WF_");

var customConfigFilePath = builder.Configuration.GetValue<string>("ConfigPath");

if (!string.IsNullOrEmpty(customConfigFilePath))
{
    builder.Configuration.AddYamlFile(customConfigFilePath, false);
}
else
{
    builder.Configuration.AddYamlFile("config.yml", true);
    builder.Configuration.AddYamlFile("config.yaml", true);
}


builder.Host.UseSerilog(
    (context, configuration) => configuration.MinimumLevel.Debug()
                                             .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<StaticAclOptions>()
       .Configure(
           options =>
           {
               options.Users = new[] {
                   new StaticAclUser {
                       Username = "localhostUser",
                       Ip = "127.0.0.1:*",
                       Acl = new[] { "default" }
                   },
                   new StaticAclUser {
                       Username = "root",
                       PlainTextPassword = "superuser",
                       Acl = new[] { "admin" }
                   }
               };

               options.Acl = new[] {
                   new StaticAclPolicy {
                       Name = "Default",
                       Access = new[] {
                           new StaticAclPolicyAccessRule {
                               Name = "*",
                               Actions =
                                   new[] { "pull" },
                               Type = "repository"
                           }
                       }
                   }
               };
           }
       );

builder.Services.AddWaterfront()
       .ConfigureEndPoints(endpoints => endpoints.TokenEndpoint = "/token")
       .WithCertificateProvider<FileTokenCertificateProvider>()
       .WithAuthentication<StaticAclAuthenticationService>()
       .WithAuthorization<StaticAclAuthorizationService>();

builder.Services.Configure<FileTokenCertificateProviderOptions>(
    opt =>
    {
        builder.Configuration.GetSection("Tokens").Bind(opt);
    });

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseWaterfront();

app.MapControllers();

app.Logger.LogInformation("Configuration path: {ConfigPath}", app.Configuration.GetValue<string>("ConfigPath"));
app.Logger.LogInformation("Crt path: {CertPath}", app.Configuration.GetValue<string>("Tokens:CertPath"));
app.Logger.LogInformation("FP opts: {@FPOpts}", app.Services.GetRequiredService<IOptions<FileTokenCertificateProviderOptions>>().Value);

app.Run();
