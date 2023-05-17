#load ../data/*.cake
#addin nuget:?package=Cake.Watch&version=0.2.3

var mainBuildTask = Task("build");

foreach (var project in projects) {
    var task = Task(project.Task("build")).Does(() => {
        Information(
            "Building project {0} v{1} using {2} configuration",
            project.Name,
            version.SemVer,
            args.Configuration
        );

        DotNetBuild(project.Path.ToString(), new DotNetBuildSettings {
            Configuration = args.Configuration,
            NoRestore = true,
            NoDependencies = true
        });

        if(
            args.Configuration is "Release" &&
            !args.NoCopyArtifacts &&
            !project.IsTest
        ) {
            var sourceDirectory = project.Directory.Combine("bin/Release/net6.0");
            var archiveName = $"{project.Name}.{version.SemVer}.zip";
            var targetFile = paths.Libraries.CombineWithFilePath(archiveName);

            Verbose(
                "Creating build output archive '{0}' from files found in directory '{1}'",
                sourceDirectory,
                targetFile
            );

            Zip(sourceDirectory, targetFile);
        }
    }).IsDependentOn(project.Task("restore"))
      .WithCriteria(!args.NoBuild);
    project.References.ForEach(dep => task.IsDependentOn(dep.Task("build")));

    mainBuildTask.IsDependentOn(task);
}
