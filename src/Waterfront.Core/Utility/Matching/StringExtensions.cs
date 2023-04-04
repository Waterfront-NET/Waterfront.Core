using DotNet.Globbing;

namespace Waterfront.Core.Utility.Matching;

public static class StringExtensions
{
    private static readonly GlobOptions DefaultGlobOptions = new GlobOptions {
        Evaluation = new EvaluationOptions { CaseInsensitive = true }
    };

    public static Glob ToGlob(this string self, GlobOptions options) => Glob.Parse(self, options);

    public static Glob ToGlob(this string self) => self.ToGlob(DefaultGlobOptions);
}
