static BuildArguments args;
args = new BuildArguments {
    Configuration = Argument("configuration", Argument("c", "Debug")),
    Target = Argument("target", Argument("t", "build")),
    NoBuild = HasArgument("no-build"),
    NoCopyArtifacts = HasArgument("no-copy-artifacts")
};

class BuildArguments {
    public string Configuration { get; init; }
    public string Target { get; init; }
    public bool NoBuild { get; init; }
    public bool NoCopyArtifacts { get; init; }
}
