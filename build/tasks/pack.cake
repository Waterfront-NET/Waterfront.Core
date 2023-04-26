#load ../data/*.cake

var mainPackTask = Task("pack");

foreach (var project in from p in projects where !p.IsTestProject() select p) {
    var task = Task(project.TaskName("pack")).Does(() => {
        Information(
            "Packing project {0} v{1} using {2} configuration",
            project.Name,
            version.SemVer,
            args.Configuration()
        );

        DotNetPack(project.Path.ToString(), new DotNetPackSettings {
            Configuration = args.Configuration(),
            NoBuild = true,
            NoDependencies = true
        });

        if(args.Configuration() is "Release" && !args.NoCopyArtifacts()) {
            var sourceDirectory = project.Directory().Combine("bin/Release");
            var packages = GetFiles(sourceDirectory.Combine($"{project.Name}.{version.SemVer}.{{nupkg,snupkg}}").ToString());
            var targetDirectory = paths.Packages();

            Verbose(
                "Copying packages from folder {0} to output folder {1}",
                sourceDirectory,
                targetDirectory
            );

            packages.ToList().ForEach(package => {
                Debug("Copying package {0}", package);

                CopyFileToDirectory(package, targetDirectory);
            });
        }
    }).IsDependentOn(project.TaskName("build"));

    mainPackTask.IsDependentOn(task);
}
