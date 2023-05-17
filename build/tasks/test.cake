#load ../data/projects.cake
#load ../data/args.cake

var mainTestTask = Task("test");

foreach(var project in from p in projects where p.IsTest select p) {
    var task = Task(project.Task("test")).Does(() => {
        DotNetTest(project.Path.ToString(), new DotNetTestSettings {
            Configuration = args.Configuration,
            NoBuild = true
        });
    }).IsDependentOn(project.Task("build"));

    mainTestTask.IsDependentOn(task);
}
