#load build/tasks/*.cake
using System.Text.Json;
#load build/data/arguments.cake
#load build/data/paths.cake
#load build/data/version.cake
#load build/data/arguments.cake

Setup(ctx => {
    Information("Starting Waterfront build v{0} on branch {1} with commit {2}", version.SemVer, version.BranchName, version.ShortSha);
    Verbose("Package version: {0}", version.NuGetVersion);
    Verbose("Full informational version: {0}", version.InformationalVersion);

    EnsureDirectoryExists(paths.Libraries());
    EnsureDirectoryExists(paths.Packages());
});

RunTarget(args.Target());
