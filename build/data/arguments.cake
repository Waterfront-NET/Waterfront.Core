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
    public bool AllConfigs() => _context.HasArgument("all-configurations") || _context.HasArgument("all-configs");
    public bool NoCopyArtifacts() => _context.HasArgument("no-copy-artifacts");
}

var args = new BuildArguments(Context);
