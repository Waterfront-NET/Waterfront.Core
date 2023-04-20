#load ../data/projects.cake
#load ../data/arguments.cake
#load ../data/paths.cake

var mainPackTask = Task("pack");

foreach (var project in projects) {
    var task = Task(project.TaskName("pack")).Does(() => {
        DotNetPack(project.Path.ToString(), new DotNetPackSettings {
            Configuration = args.Configuration(),
            NoBuild = true,
            NoDependencies = true,
            IncludeSymbols = true
        });

        var packagePath = project.Directory().Combine($"bin/{args.Configuration()}").CombineWithFilePath($"{project.Name}.1.0.0.nupkg");
        var symPackagePath = project.Directory().Combine($"bin/{args.Configuration()}").CombineWithFilePath($"{project.Name}.1.0.0.snupkg");

        CopyFile(packagePath, paths.Packages().CombineWithFilePath(packagePath.GetFilename()));
        CopyFile(packagePath, paths.Packages().CombineWithFilePath(symPackagePath.GetFilename()));
    }).IsDependentOn(project.TaskName("build"));

    mainPackTask.IsDependentOn(task);
}
