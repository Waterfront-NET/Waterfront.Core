#load ../data/projects.cake
#load ../data/arguments.cake
#load ../data/paths.cake
#load ../data/version.cake

var mainPackTask = Task("pack");

foreach (var project in projects) {
    var task = Task(project.TaskName("pack")).Does(() => {
        DotNetPack(project.Path.ToString(), new DotNetPackSettings {
            Configuration = args.Configuration(),
            NoBuild = true,
            NoDependencies = true,
            IncludeSymbols = true
        });

        var packagePath = project.Directory().Combine($"bin/{args.Configuration()}").CombineWithFilePath($"{project.Name}.{version.SemVer}.nupkg");
        var symPackagePath = project.Directory().Combine($"bin/{args.Configuration()}").CombineWithFilePath($"{project.Name}.{version.SemVer}.snupkg");

        CopyFile(packagePath, paths.Packages().CombineWithFilePath(packagePath.GetFilename()));
        CopyFile(packagePath, paths.Packages().CombineWithFilePath(symPackagePath.GetFilename()));
    }).IsDependentOn(project.TaskName("build"));

    mainPackTask.IsDependentOn(task);
}


var mainLibTask = Task("lib");

foreach(var project in projects) {
  var task = Task(project.TaskName("lib")).Does(() => {
    var sourceDir = project.Bin(args.Configuration(), "net6.0");
    var targetArchive = paths.Libraries().CombineWithFilePath($"{project.Name}_{version.SemVer}_Release_net6.0.zip");
     Zip(sourceDir, targetArchive);
     Information("Packing {0} directory to target archive: {1}", sourceDir, targetArchive);
  }).IsDependentOn(project.TaskName("build"));

  mainLibTask.IsDependentOn(task);
}
