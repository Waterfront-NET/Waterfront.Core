#load ../data/projects.cake
#load ../data/arguments.cake
#load ../data/paths.cake
#load ../data/version.cake

var mainPackTask = Task("pack");

foreach (var project in projects) {
    var task = Task(project.TaskName("pack")).Does(() => {
        Information("Running .NET pack for project {0} with configuration {1}", project.Name, args.Configuration());
        DotNetPack(project.Path.ToString(), new DotNetPackSettings {
            Configuration = args.Configuration(),
            NoBuild = true,
            NoDependencies = true
        });
    }).IsDependentOn(project.TaskName("build"));

    mainPackTask.IsDependentOn(task);
}


// var mainLibTask = Task("lib");

// foreach(var project in projects) {
//   var task = Task(project.TaskName("lib")).Does(() => {
//     var sourceDir = project.Bin(args.Configuration(), "net6.0");
//     var targetArchive = paths.Libraries().CombineWithFilePath($"{project.Name}_{version.NuGetVersion}_{args.Configuration()}_net6.0.zip");
//      Zip(sourceDir, targetArchive);
//      Information("Packing {0} directory to target archive: {1}", sourceDir, targetArchive);
//   }).IsDependentOn(project.TaskName("build"));

//   mainLibTask.IsDependentOn(task);
// }
