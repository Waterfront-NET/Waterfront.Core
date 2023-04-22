#load ../data/*.cake

var mainPkgTask = Task("artifacts/pkg");

foreach(var project in projects) {
    var task = Task(project.TaskName("artifacts/pkg")).Does(() => {
        Information("Copying NuGet packages for project {0} to output directory", project.Name);

        var pkgPath = project.PackagePath(args.Configuration(), version.SemVer);
        var spkgPath = project.SymbolPackagePath(args.Configuration(), version.SemVer);

        Verbose("Source package path: {0}", pkgPath);
        Verbose("Source symbol package path: {0}", spkgPath);

        CopyFileToDirectory(pkgPath, paths.Packages());
        CopyFileToDirectory(spkgPath, paths.Packages());
    }).IsDependentOn(project.TaskName("pack"))
      .WithCriteria(args.Configuration() is "Release", "Configuration is not 'Release'");

    mainPkgTask.IsDependentOn(task);
}

var mainLibTask = Task("artifacts/lib");

foreach(var project in from p in projects where !p.IsTestProject() select p) {
    var task = Task(project.TaskName("artifacts/lib")).Does(() => {
        var dirs = GetDirectories(project.Directory().Combine("bin/" + args.Configuration() + "/*").ToString());

        Information("Source directories found: [{0}]", string.Join(", ", dirs.Select(dir => dir.GetDirectoryName())));

        dirs.ToList().ForEach(dir => {
            var archiveName = $"{project.Name}_{version.SemVer}_{dir.GetDirectoryName().ToString()}.zip";
            var targetArchive = paths.Libraries().CombineWithFilePath(archiveName);

            Information("Will create archive {0} from directory {1}", archiveName, dir);

            Zip(dir, targetArchive);
        });
    }).IsDependentOn(project.TaskName("build"))
      .WithCriteria(args.Configuration() is "Release", "Configuration is not 'Release'");


    mainLibTask.IsDependentOn(task);
}

Task("artifacts/push-nuget-pkg").Does(() => {
    var packageFiles = GetFiles(paths.Packages().Combine("*").ToString());

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

Task("artifacts/push-release-asset").Does(() => {
    var libFiles = GetFiles(paths.Libraries().Combine("*").ToString());

    libFiles.ToList().ForEach(file => {

    });
});
