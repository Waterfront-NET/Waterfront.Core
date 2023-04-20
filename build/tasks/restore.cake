#load ../data/projects.cake

var mainRestoreTask = Task("restore");

foreach (var project in projects) {
    var task = Task(project.TaskName("restore")).Does(() => {
        DotNetRestore(project.Path.ToString(), new DotNetRestoreSettings {
            NoDependencies = true
        });
    });

    mainRestoreTask.IsDependentOn(task);
}
