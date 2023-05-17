#load ../data/projects.cake
#load ../data/args.cake

var mainRestoreTask = Task("restore");

foreach (var project in projects) {
    var task = Task(project.Task("restore")).Does(() => {
        DotNetRestore(project.Path.ToString(), new DotNetRestoreSettings {
            NoDependencies = true
        });
    }).WithCriteria(() => !args.NoBuild);

    mainRestoreTask.IsDependentOn(task);
}
