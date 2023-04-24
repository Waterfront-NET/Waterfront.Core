class BuildArguments {
    private readonly ICakeContext _context;

    public BuildArguments(ICakeContext context) {
        _context = context;
    }

    public string Configuration() => _context.Argument("configuration", _context.Argument("c", "Debug"));
    public string Target() => _context.Argument("target", _context.Argument("t", "build"));
    public bool NoIncremental() => _context.Argument("no-incremental", false);
    public bool NoBuild() => _context.HasArgument("no-build");
    public bool IsCi() => _context.GitHubActions().IsRunningOnGitHubActions;

    public string GithubToken() => _context.EnvironmentVariable("GITHUB_TOKEN", string.Empty);
    public string RepoOwner() => _context.EnvironmentVariable("REPO_OWNER", string.Empty);
    public string RepoName() => _context.EnvironmentVariable("REPO_NAME", string.Empty);
}

var args = new BuildArguments(Context);
