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

        if(args.Configuration is "Debug" && !args.NoCopyArtifacts) {
            var sourceDirectory = project.Directory.Combine("bin/Debug");
            var packageGlobPattern = sourceDirectory.Combine($"{project.Name}.{version.SemVer}.nupkg").ToString();
            Debug("Glob pattern: {0}", packageGlobPattern);
            var packages = GetFiles(packageGlobPattern);
            var localRepoDirectory = DirectoryPath.FromString(EnvironmentVariable("USERPROFILE")).Combine(".nuget/packages");

            Verbose("Pushing debug version of packages to local NuGet repository: {0}", localRepoDirectory);

            Debug("Packages: {0}", string.Join(", ", packages));

            foreach(var packagePath in packages) {
                Debug("Pushing package {0}", packagePath);
                var targetPackageName = project.Name;
                var targetPackageVersion = version.SemVer;

                var targetDirectory = localRepoDirectory.Combine(targetPackageName).Combine(targetPackageVersion);
                EnsureDirectoryDoesNotExist(targetDirectory);

                DotNetNuGetPush(packagePath, new DotNetNuGetPushSettings {
                    Source = localRepoDirectory.ToString()
                });
            }
        }
    }).IsDependentOn(project.Task("build"));

    mainPackTask.IsDependentOn(task);
}
