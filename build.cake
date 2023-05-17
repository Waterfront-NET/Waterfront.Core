#load build/tasks/*.cake
#load build/data/args.cake
#load build/data/paths.cake
#load build/data/version.cake

using System.Collections;
using System.Text.Json;

Setup(ctx => {
    Information("Starting Waterfront build v{0} on branch {1} with commit {2}", version.SemVer, version.BranchName, version.ShortSha);
    Verbose("Package version: {0}", version.NuGetVersion);
    Verbose("Full informational version: {0}", version.InformationalVersion);

    Information("Setting version environment variables...");

    Environment.SetEnvironmentVariable("GitVersion_SemVer", version.SemVer);
    Environment.SetEnvironmentVariable("GitVersion_InformationalVersion", version.InformationalVersion);
    Environment.SetEnvironmentVariable("GitVersion_NuGetVersion", version.NuGetVersion);
    Environment.SetEnvironmentVariable("GitVersion_AssemblyVersion", version.AssemblySemVer);
    Environment.SetEnvironmentVariable("GitVersion_AssemblyFileVersion", version.AssemblySemFileVer);

    foreach(DictionaryEntry env in Environment.GetEnvironmentVariables()) {

        if(((string)env.Key).StartsWith("GitVersion_")) {
            Verbose("GitVersion variable: {0}={1}", env.Key, env.Value);
        }
    }

    EnsureDirectoryExists(paths.Libraries);
    EnsureDirectoryExists(paths.Packages);
});

RunTarget(args.Target);
