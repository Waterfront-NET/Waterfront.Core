using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Waterfront.Acl.Static;
using Waterfront.Acl.Static.Models;
using Waterfront.Acl.Static.Options;
using Waterfront.AspNetCore.Configuration;
using Waterfront.AspNetCore.Extensions;
using Waterfront.AspNetCore.Services.Authentication;
using Waterfront.AspNetCore.Services.Authorization;
using Waterfront.Core.Configuration;
using Waterfront.Core.Security.Cryptography;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables("WF_");

string? customConfigFilePath = builder.Configuration.GetValue<string>("ConfigPath");

if ( !string.IsNullOrEmpty(customConfigFilePath) )
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

builder.Services.AddScoped<TokenRequestAuthenticationService>()
       .AddScoped<TokenRequestAuthorizationService>();

WaterfrontBuilder waterfrontBuilder = builder.Services.AddWaterfront()
                                             .ConfigureTokenOptions(
                                                 options => {
                                                     builder.Configuration.GetSection("Tokens")
                                                            .Bind(options);
                                                 }
                                             )
                                             .ConfigureEndPoints(
                                                 endpoints => endpoints.TokenEndpoint = "/token"
                                             )
                                             .WithCertificateProvider<FileTokenCertificateProvider,
                                                 FileTokenCertificateProviderOptions>(
                                                 options => {
                                                     builder.Configuration.GetSection("Tokens")
                                                            .Bind(options);
                                                 }
                                             );

StaticAclUser[]? staticAclUsers = builder.Configuration.GetSection("Users").Get<StaticAclUser[]>();
StaticAclPolicy[]? staticAclPolicies =
builder.Configuration.GetSection("Acl").Get<StaticAclPolicy[]>();

if ( staticAclUsers is { Length: not 0 } )
{
    waterfrontBuilder.WithAuthentication<StaticAclAuthenticationService>();
    builder.Services.AddOptions<StaticAclOptions>()
           .Configure(
               options => {
                   options.Users = staticAclUsers;
               }
           );
}

if ( staticAclPolicies is { Length: not 0 } )
{
    waterfrontBuilder.WithAuthorization<StaticAclAuthorizationService>();
    builder.Services.AddOptions<StaticAclOptions>()
           .Configure(
               options => {
                   options.Acl = staticAclPolicies;
               }
           );
}

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseWaterfront();

app.MapControllers();

app.Logger.LogInformation(
    "Configuration path: {ConfigPath}",
    app.Configuration.GetValue<string>("ConfigPath")
);
app.Logger.LogInformation(
    "TokenOptions: {@TokenOptions}",
    app.Services.GetRequiredService<IOptions<TokenOptions>>().Value
);
app.Logger.LogInformation(
    "FP opts: {@FPOpts}",
    app.Services.GetRequiredService<IOptions<FileTokenCertificateProviderOptions>>().Value
);

app.Run();
