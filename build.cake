#load build/tasks/*.cake
using System.Text.Json;
#load build/data/arguments.cake
#load build/data/paths.cake
#load build/data/version.cake
#load build/data/arguments.cake

Setup(ctx => {

    Information("Running build v{0}", version.SemVer);
    Information("Package version: {0}", version.NuGetVersion);
    Verbose("Informational version: {0}", version.InformationalVersion);

    EnsureDirectoryExists(paths.Libraries());
    EnsureDirectoryExists(paths.Packages());

    Information("GitHub actions debug info:\n{0}",
    JsonSerializer.Serialize(GitHubActions.Environment.Runtime));
});

RunTarget(args.Target());


// RunTarget(target);
