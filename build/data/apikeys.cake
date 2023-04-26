static BuildApiKeys apiKeys;
apiKeys = new BuildApiKeys {
    GithubToken = EnvironmentVariable("GITHUB_TOKEN"),
    NugetApiKey = EnvironmentVariable("NUGET_API_KEY"),
    MygetApiKey = EnvironmentVariable("MYGET_API_KEY")
};

class BuildApiKeys {
    public string GithubToken { get; init; }
    public string NugetApiKey { get; init; }
    public string MygetApiKey { get; init; }
}
