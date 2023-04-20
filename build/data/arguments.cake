class BuildArguments {
    private readonly ICakeContext _context;

    public BuildArguments(ICakeContext context) {
        _context = context;
    }

    public string Configuration() => _context.Argument("configuration", _context.Argument("c", "Debug"));
    public string Target() => _context.Argument("target", _context.Argument("t", "build"));
    public bool NoIncremental() => _context.Argument("no-incremental", false);
}

var args = new BuildArguments(Context);
