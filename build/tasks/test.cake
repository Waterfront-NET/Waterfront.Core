#load ../data/projects.cake
#load ../data/arguments.cake

var mainTestTask = Task("test");

foreach(var project in from p in projects where p.IsTestProject() select p) {
    var task = Task(project.TaskName("test")).Does(() => {
        DotNetTest(project.Path.ToString(), new DotNetTestSettings {
            Configuration = args.Configuration(),
            NoBuild = true
        });
    }).IsDependentOn(project.TaskName("build"));

    mainTestTask.IsDependentOn(task);
}
