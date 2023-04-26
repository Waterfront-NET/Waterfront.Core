#load paths.cake
using System.Xml;

static List<BuildProject> projects;

projects = new List<BuildProject> {
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
};

class BuildProject {
    private List<BuildProject> _dependencies;

    public string Name { get; init; }
    public FilePath Path { get; init; }

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
        return Name.Replace("Waterfront.", string.Empty).ToLowerInvariant();
    }

    public string TaskName(string task) {
        return $":{ShortName()}:{task}";
    }

    public bool IsTestProject() => Directory().GetParent()
                                              .GetDirectoryName() is "test";

    public List<BuildProject> Dependencies() {
        if(_dependencies == null) {
            _dependencies = new List<BuildProject>();
            var doc = new XmlDocument();
            doc.Load(Path.ToString());

            var projectReferenceNodes = doc.SelectNodes("//ProjectReference");
            if(projectReferenceNodes != null) {
                foreach(XmlNode node in projectReferenceNodes) {
                    var includeAttr = node.Attributes.GetNamedItem("Include");
                    if(includeAttr == null) {
                        throw new Exception($"Found project reference node in project {Name}, but could not find Include attribute");
                    }

                    string relativePath = includeAttr.Value;
                    FilePath absolutePath = FilePath.FromString(relativePath).MakeAbsolute(Directory());
                    BuildProject dependencyProject = projects.Find(project => project.Path == absolutePath);

                    if(dependencyProject == null) {
                        throw new Exception($"Dependency {absolutePath.GetFilenameWithoutExtension()} of the project {Name} could not be found");
                    }

                    _dependencies.Add(dependencyProject);
                }
            }
        }

        return _dependencies;
    }
}
