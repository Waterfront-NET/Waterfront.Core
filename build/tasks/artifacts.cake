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

        if(args.IsCi()) {
            Information("Running on GitHub Actions, will upload artifacts [{0}, {1}]", pkgPath.GetFilename(), spkgPath.GetFilename());
            GitHubActions.Commands.UploadArtifact(paths.Packages().CombineWithFilePath(pkgPath.GetFilename()), pkgPath.GetFilename().ToString()).Wait();
            GitHubActions.Commands.UploadArtifact(paths.Packages().CombineWithFilePath(spkgPath.GetFilename()), spkgPath.GetFilename().ToString()).Wait();
        }
    }).IsDependentOn(project.TaskName("pack"))
      .WithCriteria(args.Configuration() is "Release", "Configuration is not 'Release'");

    mainPkgTask.IsDependentOn(task);
}

var mainLibTask = Task("artifacts/lib");


foreach(var project in projects) {
    var task = Task(project.TaskName("artifacts/lib")).Does(() => {
        var dirs = GetDirectories(project.Directory().Combine("bin/" + args.Configuration() + "/*").ToString());

        Information("Source directories found: [{0}]", string.Join(", ", dirs.Select(dir => dir.GetDirectoryName())));

        dirs.ToList().ForEach(dir => {
            var archiveName = $"{project.Name}_{version.SemVer}_{dir.GetDirectoryName().ToString()}.zip";
            var targetArchive = paths.Libraries().CombineWithFilePath(archiveName);

            Information("Will create archive {0} from directory {1}");

            Zip(dir, targetArchive);

            if(args.IsCi()) {
                Information("Running on GitHub Actions, will upload artifact {0}", archiveName);
                GitHubActions.Commands.UploadArtifact(targetArchive, archiveName).Wait();
            }
        });
    }).IsDependentOn(project.TaskName("build"))
      .WithCriteria(args.Configuration() is "Release", "Configuration is not 'Release'");


    mainLibTask.IsDependentOn(task);
}
