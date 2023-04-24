#load paths.cake

var projects = new List<BuildProject> {
    new BuildProject {
        Name = "Waterfront.Common",
        Path = paths.Source().CombineWithFilePath("Waterfront.Common/Waterfront.Common.csproj")
    },
    new BuildProject {
        Name = "Waterfront.Core",
        Path = paths.Source().CombineWithFilePath("Waterfront.Core/Waterfront.Core.csproj"),
        DependencyNames = {"Waterfront.Common"}
    },
    new BuildProject {
        Name = "Waterfront.Common.Tests",
        Path = paths.Tests().CombineWithFilePath("Waterfront.Common.Tests/Waterfront.Common.Tests.csproj"),
        DependencyNames = {"Waterfront.Common"}
    },
    new BuildProject {
        Name = "Waterfront.Core.Tests",
        Path = paths.Tests().CombineWithFilePath("Waterfront.Core.Tests/Waterfront.Core.Tests.csproj"),
        DependencyNames = {"Waterfront.Core"}
    }
};

projects.ForEach(p => p.ResolveDependencies(projects));

class BuildProject {
    public string Name { get; init; }
    public FilePath Path { get; init; }
    public List<BuildProject> Dependencies { get; }
    public List<string> DependencyNames { get; }

    public BuildProject() {
        Dependencies = new List<BuildProject>();
        DependencyNames = new List<string>();
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

    public bool IsTestProject() => Name.EndsWith(".Tests");

    public BuildProject GetMainProject() => IsTestProject() ? Dependencies.First() : throw new InvalidOperationException("Not a test project");

    public void ResolveDependencies(List<BuildProject> projects) {
        Dependencies.Clear();
        Dependencies.AddRange(DependencyNames.Select(name => {
            var dep = projects.Find(x => x.Name == name);
            if(dep == null) {
                throw new Exception($"Depencency project of {Name} with name {name} was not found in the list of projects");
            }
            return dep;
        }));
    }
}
