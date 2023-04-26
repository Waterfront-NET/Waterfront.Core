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

    Information("Setting version environment variables...");

    Environment.SetEnvironmentVariable("SEMVER", version.SemVer);
    Environment.SetEnvironmentVariable("INFO_VER", version.InformationalVersion);
    Environment.SetEnvironmentVariable("NUGET_VER", version.NuGetVersion);
    Environment.SetEnvironmentVariable("GIT_BRANCH", version.BranchName);
    Environment.SetEnvironmentVariable("BRANCH", version.BranchName);
    Environment.SetEnvironmentVariable("GIT_COMMIT_HASH", version.Sha);
    Environment.SetEnvironmentVariable("GIT_COMMIT_HASH_SHORT", version.ShortSha);
    Environment.SetEnvironmentVariable("COMMIT_SHORT_SHA", version.ShortSha);

    Verbose("Version environment variables were set:\nSEMVER={0}\nINFO_VER={1}\nNUGET_VER={2}\nGIT_BRANCH={3}\nBRANCH={4}\nGIT_COMMIT_HASH={5}\nGIT_COMMIT_HASH_SHORT={6}\nCOMMIT_SHORT_SHA={7}", version.SemVer,version.InformationalVersion, version.NuGetVersion, version.BranchName, version.BranchName,version.Sha,version.ShortSha,version.ShortSha);

    EnsureDirectoryExists(paths.Libraries());
    EnsureDirectoryExists(paths.Packages());
});

RunTarget(args.Target());
