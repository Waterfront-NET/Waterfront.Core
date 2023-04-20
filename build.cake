#load build/tasks/*.cake
#load build/data/arguments.cake
#load build/data/paths.cake

Setup(ctx => {
    EnsureDirectoryExists(paths.Libraries());
    EnsureDirectoryExists(paths.Packages());
});

/* Setup(ctx => {
    var rootDir = ctx.Environment.WorkingDirectory;
    var srcDir = rootDir.Combine("src");
    var testDir = rootDir.Combine("tests");

    var state = new BuildState {
        Paths = new BuildPaths {
            Root = rootDir,
            Solution = rootDir.CombineWithFilePath("Waterfront.sln")
        },
        Projects = {
            new BuildProject {
                Name = "Waterfront.Core",
                ShortName = "core",
                Path = srcDir.Combine("Waterfront.Core").CombineWithFilePath("Waterfront.Core.csproj")
            },
            new BuildProject {
                Name = "Waterfront.Common",
                ShortName = "common",
                Path = srcDir.Combine("Waterfront.Common").CombineWithFilePath("Waterfront.Common.csproj")
            }
        }
    };

    var gitversion = GitVersion();

    Information("GitVersion data:\n{0}", JsonSerializer.Serialize(
    gitversion,
    new JsonSerializerOptions { WriteIndented = true }
    ));

    return state;
}); */

RunTarget(args.Target());


// RunTarget(target);
