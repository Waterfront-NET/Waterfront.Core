#load ../data/projects.cake
#load ../data/arguments.cake
#load restore.cake
#addin nuget:?package=Cake.Watch&version=0.2.3

var mainBuildTask = Task("build");

foreach (var project in projects) {
    var task = Task(project.TaskName("build")).Does(() => {
        DotNetBuild(project.Path.ToString(), new DotNetBuildSettings {
            Configuration = args.Configuration(),
            NoRestore = true,
            NoDependencies = true,
            NoIncremental = args.NoIncremental()
        });
    }).IsDependentOn(project.TaskName("restore"))
      .IsDependentOn("set-version-env")
      .WithCriteria(() => !args.NoBuild());
    project.Dependencies.ForEach(dep => task.IsDependentOn(dep.TaskName("build")));

    mainBuildTask.IsDependentOn(task);
}
