#load ../data/paths.cake
#load ../data/args.cake
#load ../data/projects.cake

var cleanAllTask = Task("clean");

var cleanSolutionTask = Task("clean/sln").Does(() => {
    DotNetClean(paths.Solution.ToString(), new DotNetCleanSettings {
        Configuration = args.Configuration
    });
});

projects.ForEach(project => {
    var task = Task(project.Task("clean")).Does(() => {
        DotNetClean(project.Path.ToString(), new DotNetCleanSettings {
            Configuration = args.Configuration
        });
    });

    cleanAllTask.IsDependentOn(task);
});

cleanAllTask.IsDependentOn(Task("clean/artifacts/lib").Does(() => {
    CleanDirectory(paths.Libraries);
})).IsDependentOn(Task("clean/artifacts/pkg").Does(() => {
    CleanDirectory(paths.Packages);
}));
