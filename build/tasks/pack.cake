#load ../data/*.cake

var mainPackTask = Task("pack");

foreach (var project in from p in projects where !p.IsTest select p) {
    var task = Task(project.Task("pack")).Does(() => {
        Information(
            "Packing project {0} v{1} using {2} configuration",
            project.Name,
            version.SemVer,
            args.Configuration
        );

        DotNetPack(project.Path.ToString(), new DotNetPackSettings {
            Configuration = args.Configuration,
            NoBuild = true,
            NoDependencies = true
        });

        if(args.Configuration is "Release" && !args.NoCopyArtifacts) {
            var sourceDirectory = project.Directory.Combine("bin/Release");
            var packages = GetFiles(sourceDirectory.Combine($"{project.Name}.{version.SemVer}.{{nupkg,snupkg}}").ToString());
            var targetDirectory = paths.Packages;

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

        if(args.Configuration is "Debug" && !args.NoLocalPush) {
            var packagePath = project.PackagePath(args.Configuration, version.SemVer);
            var localRepoPath = DirectoryPath.FromString(EnvironmentVariable("USERPROFILE")).Combine(".nuget/packages");
            Verbose(
                "Pushing debug version of package {0} ({1}) to local repository - {2}",
                project.Name,
                packagePath,
                localRepoPath
            );

            var targetDirectoryPath = localRepoPath.Combine(project.Name.ToLowerInvariant()).Combine(version.SemVer);

            EnsureDirectoryDoesNotExist(targetDirectoryPath);

            DotNetNuGetPush(packagePath, new DotNetNuGetPushSettings {
                Source = localRepoPath.ToString()
            });
        }
    }).IsDependentOn(project.Task("build"));

    mainPackTask.IsDependentOn(task);
}
