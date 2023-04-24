#load ../data/version.cake

Task("set-version-env").Does(() => {
    Information("Setting version environment variables...");

    Environment.SetEnvironmentVariable("SEMVER", version.SemVer);
    Environment.SetEnvironmentVariable("INFO_VER", version.InformationalVersion);
    Environment.SetEnvironmentVariable("NUGET_VER", version.NuGetVersion);
    Environment.SetEnvironmentVariable("GIT_BRANCH", version.BranchName);
    Environment.SetEnvironmentVariable("BRANCH", version.BranchName);
    Environment.SetEnvironmentVariable("GIT_COMMIT_HASH", version.Sha);
    Environment.SetEnvironmentVariable("GIT_COMMIT_HASH_SHORT", version.ShortSha);
    Environment.SetEnvironmentVariable("COMMIT_SHORT_SHA", version.ShortSha);

    Verbose("Version environment variables were set:\nSEMVER={0}\nINFO_VER={1}\nNUGET_VER={2}\nGIT_BRANCH={3}\nBRANCH={4}\nGIT_COMMIT_HASH={5}\nGIT_COMMIT_HASH_SHORT={6}\nCOMMIT_SHORT_SHA={7}", version.SemVer,version.InformationalVersion, version.NuGetVersion, version.BranchName, version.BranchName,version.Sha,version.ShortSha,version.ShortSha);
});
