#load ../data/paths.cake
#load ../data/projects.cake
#addin nuget:?package=Cake.Watch&version=0.2.3

Task("watch").Does(() => {
    var sourceDirPath = paths.Source().ToString();
    var watchSettings = new WatchSettings {
        Path = sourceDirPath,
        Pattern = "*.cs",
        Recursive = true
    };

    Information("Watching for file changes...");

    Watch(watchSettings, fileChanges => {
        Information("Files updated:\n{0}\nRebuilding libraries...", string.Join(", ", fileChanges.Select(change => change.FullPath)));
        foreach(var project in projects) {
            DotNetBuild(project.Path.ToString(), new DotNetBuildSettings {
                Configuration = "Debug"
            });
        }
    });
}).IsDependentOn(":common:build")
  .IsDependentOn(":core:build");
