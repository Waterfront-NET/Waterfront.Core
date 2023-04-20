#load paths.cake

var projects = new[] {
    new BuildProject {
        Name = "Waterfront.Common",
        Path = paths.Source().CombineWithFilePath("Waterfront.Common/Waterfront.Common.csproj")
    },
    new BuildProject {
        Name = "Waterfront.Core",
        Path = paths.Source().CombineWithFilePath("Waterfront.Core/Waterfront.Core.csproj")
    }
};

projects[1].DependsOn(projects[0]);


class BuildProject {
    public string Name { get; init; }
    public FilePath Path { get; init; }
    public List<BuildProject> Dependencies { get; }

    public BuildProject() {
        Dependencies = new List<BuildProject>();
    }

    public DirectoryPath Directory() => Path.GetDirectory();
    public string ShortName() {
        return Name.Replace("Waterfront.", "").ToLowerInvariant();
    }
    public string TaskName(string task) {
        return $":{ShortName()}:{task}";
    }
    public BuildProject DependsOn(BuildProject project) {
        Dependencies.Add(project);
        return this;
    }
}
