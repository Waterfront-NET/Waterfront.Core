var paths = new BuildPaths { Root = Context.Environment.WorkingDirectory };

class BuildPaths {
    public DirectoryPath Root { get; init; }
    public DirectoryPath Source() {
        return Root.Combine("src");
    }
    public DirectoryPath Tests() {
        return Root.Combine("test");
    }
    public FilePath Solution() {
        return Root.CombineWithFilePath("Waterfront.sln");
    }

    public DirectoryPath Packages() {
        return Root.Combine("artifacts/pkg");
    }

    public DirectoryPath Libraries() {
        return Root.Combine("artifacts/lib");
    }
}
