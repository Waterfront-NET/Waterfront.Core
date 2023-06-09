#load ../data/*.cake

var mainPkgTask = Task("artifacts/pkg");

foreach(var project in projects.Where(p => !p.IsTest)) {
    var task = Task(project.Task("artifacts/pkg")).Does(() => {
        Information("Copying NuGet packages for project {0} to output directory", project.Name);

        var pkgPath = project.PackagePath(args.Configuration, version.SemVer);
        var spkgPath = project.SymbolPackagePath(args.Configuration, version.SemVer);

        Verbose("Source package path: {0}", pkgPath);
        Verbose("Source symbol package path: {0}", spkgPath);

        CopyFileToDirectory(pkgPath, paths.Packages);
        CopyFileToDirectory(spkgPath, paths.Packages);
    }).IsDependentOn(project.Task("pack"))
      .WithCriteria(args.Configuration is "Release", "Configuration is not 'Release'");

    mainPkgTask.IsDependentOn(task);
}

var mainLibTask = Task("artifacts/lib");

foreach(var project in from p in projects where !p.IsTest select p) {
    var task = Task(project.Task("artifacts/lib")).Does(() => {
        var dirs = GetDirectories(project.Directory.Combine("bin/" + args.Configuration + "/*").ToString());

        Information("Source directories found: [{0}]", string.Join(", ", dirs.Select(dir => dir.GetDirectoryName())));

        dirs.ToList().ForEach(dir => {
            var archiveName = $"{project.Name}.{version.SemVer}.zip";
            var targetArchive = paths.Libraries.CombineWithFilePath(archiveName);

            Information("Will create archive {0} from directory {1}", archiveName, dir);

            Zip(dir, targetArchive);
        });
    }).IsDependentOn(project.Task("build"))
      .WithCriteria(args.Configuration is "Release", "Configuration is not 'Release'");


    mainLibTask.IsDependentOn(task);
}

Task("artifacts/push/nuget").Does(() => {
    var packages = GetFiles(paths.Packages.Combine("*.nupkg").ToString()).ToList();
    packages.ForEach(package => {
        NuGetPush(package, new NuGetPushSettings {
            ApiKey = EnvironmentVariable("NUGET_API_KEY", string.Empty),
            Source = "https://api.nuget.org/v3/index.json"
        });
    });
});

Task("artifacts/push/github").Does(() => {
    if(!DotNetNuGetHasSource("github")) {
        DotNetNuGetAddSource("github", new DotNetNuGetSourceSettings {
            UserName = "USERNAME",
            Password = EnvironmentVariable("GITHUB_NUGET_PKG_TOKEN"),
            StorePasswordInClearText = true,
            Source = "https://nuget.pkg.github.com/Waterfront-NET/index.json"
        });
    }

    GetFiles(
    paths.Packages
         .Combine("*.nupkg")
         .ToString()
    ).ToList()
     .ForEach(file => {
        /* NuGetPush(file, new NuGetPushSettings {
            ApiKey = EnvironmentVariable("GITHUB_TOKEN", string.Empty),
            Source = "https://nuget.pkg.github.com/Waterfront-NET/index.json",
        }); */

        DotNetNuGetPush(file, new DotNetNuGetPushSettings {
            Source = "github",
            ApiKey = EnvironmentVariable("GITHUB_NUGET_PKG_TOKEN")
        });
    });
});

Task("artifacts/push/release-assets");

Task("artifacts/push-nuget-pkg").Does(() => {
    var packageFiles = GetFiles(paths.Packages.Combine("*").ToString());

    if(packageFiles.Count() is 0) {
        throw new Exception("Nothing to push");
    }

    Information("Will push the following packages to NuGet: [{0}]", string.Join(", ", packageFiles.Select(file => file.GetFilename())));

    packageFiles.ToList().ForEach(file => {
        NuGetPush(file, new NuGetPushSettings {
            ApiKey = EnvironmentVariable("NUGET_API_KEY", string.Empty),
            Source = "https://api.nuget.org/v3/index.json"
        });
    });
}).IsDependentOn("artifacts/pkg");

/* Task("artifacts/push-release-assets").Does(() => {
    var libFiles = GetFiles(paths.Libraries().Combine("*").ToString());

    libFiles.ToList().ForEach(file => {
        GitReleaseManagerAddAssets(
            args.GithubToken(),
            args.RepoOwner(),
            args.RepoName(),
            version.SemVer,
            string.Empty,
            new GitReleaseManagerAddAssetsSettings {

            }
        );
    });
}); */
