#load ../data/version.cake

Task("set-version").Does(() => {
    Environment.SetEnvironmentVariable("SEMVER", version.SemVer);
    Environment.SetEnvironmentVariable("INFO_VER", version.InformationalVersion);
    Environment.SetEnvironmentVariable("COMMIT_SHA", version.Sha);
    Environment.SetEnvironmentVariable("COMMIT_SHORT_SHA", version.ShortSha);
    Environment.SetEnvironmentVariable("BRANCH", version.BranchName);

    Information("Done setting version information:\n" +
    "Semantic version: {0}\n" +
    "Informational version: {1}\n" +
    "Branch name: {2}\n" +
    "Commit sha (short): {3}",
    version.SemVer,
    version.InformationalVersion,
    version.BranchName,
    version.ShortSha);
 });
