#load build/tasks/*.cake
#load build/data/arguments.cake
#load build/data/paths.cake
#load build/data/version.cake

Setup(ctx => {

    Information("Running build v{0}", version.SemVer);
    Information("Package version: {0}", version.NuGetVersion);
    Verbose("Informational version: {0}", version.InformationalVersion);

    EnsureDirectoryExists(paths.Libraries());
    EnsureDirectoryExists(paths.Packages());
});

RunTarget(args.Target());


// RunTarget(target);
