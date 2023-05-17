static BuildPaths paths;
paths = new BuildPaths { Root = Context.Environment.WorkingDirectory };

class BuildPaths {
    public DirectoryPath Root { get; init; }

    public FilePath Solution => Root.CombineWithFilePath("Waterfront.Core.sln");

    public DirectoryPath Source => Root.Combine("src");
    public DirectoryPath Tests => Root.Combine("test");

    public DirectoryPath Packages => Root.Combine("artifacts/pkg");
    public DirectoryPath Libraries => Root.Combine("artifacts/lib");
}
