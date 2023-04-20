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

    public DirectoryPath Bin(string configuration = "Debug", string framework = "net6.0") =>
        Directory().Combine("bin")
                   .Combine(configuration)
                   .Combine(framework);

    public FilePath PackagePath(string configuration, string version) => Directory().Combine("bin")
                                                                                    .Combine(configuration)
                                                                                    .CombineWithFilePath($"{Name}.{version}.nupkg");

    public FilePath SymbolPackagePath(string configuration, string version) => Directory().Combine("bin")
                                                                                          .Combine(configuration)
                                                                                          .CombineWithFilePath($"{Name}.{version}.snupkg");

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
