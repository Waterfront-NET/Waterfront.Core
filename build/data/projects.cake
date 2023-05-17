#addin nuget:?package=Cake.Incubator&version=8.0.0
#load paths.cake
#load args.cake

using System.Xml;

static List<BuildProject> projects;

/* projects = new List<BuildProject> {
    new BuildProject {
        Name = "Waterfront.Common",
        Path = paths.Source().CombineWithFilePath("Waterfront.Common/Waterfront.Common.csproj")
    },
    new BuildProject {
        Name = "Waterfront.Core",
        Path = paths.Source().CombineWithFilePath("Waterfront.Core/Waterfront.Core.csproj")
    },
    new BuildProject {
        Name = "Waterfront.Common.Tests",
        Path = paths.Tests().CombineWithFilePath("Waterfront.Common.Tests/Waterfront.Common.Tests.csproj"),
    },
    new BuildProject {
        Name = "Waterfront.Core.Tests",
        Path = paths.Tests().CombineWithFilePath("Waterfront.Core.Tests/Waterfront.Core.Tests.csproj"),
    }
}; */

projects = ParseSolution(paths.Solution).GetProjects()
.Where(solutionProject => solutionProject.IsType(ProjectType.CSharp))
.ToList()
.Select(solutionProject => {
    var path = solutionProject.Path.MakeAbsolute(paths.Solution.GetDirectory());

    var parsedProject = ParseProject(path, args.Configuration);

    var buildProject = new BuildProject {
        Name = solutionProject.Name,
        Path = path,
        IsTest = parsedProject.IsTestProject()
    };

    parsedProject.ProjectReferences.ToList().ForEach(projectReference => {
        var name = projectReference.FilePath.GetFilenameWithoutExtension().ToString();

        var referenceBuildProject = new BuildProject {
            Name = name,
            Path = projectReference.FilePath
        };

        buildProject.References.Add(referenceBuildProject);
    });

    return buildProject;
}).ToList();

class BuildProject {
    public string Name { get; init; }
    public string Shortname => Name.Replace("Waterfront.", string.Empty).ToLowerInvariant();
    public FilePath Path { get; init; }
    public DirectoryPath Directory => Path.GetDirectory();

    public bool IsTest { get; init; }

    public List<BuildProject> References { get; } = new List<BuildProject>();

    public string Task(string task) {
        return $":{Shortname}:{task}";
    }

    public FilePath PackagePath(string configuration, string version) {
        return Directory.CombineWithFilePath($"bin/{configuration}/{Name}.{version}.nupkg");
    }

    public FilePath SymbolPackagePath(string configuration, string version) {
        return Directory.CombineWithFilePath($"bin/{configuration}/{Name}.{version}.snupkg");
    }
}
