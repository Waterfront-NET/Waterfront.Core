using DotNet.Globbing;

namespace Waterfront.Core.Utility.Matching;

public static class StringExtensions
{
    public static Glob ToGlob(this string self, GlobOptions options)
    {
        return Glob.Parse(self, options);
    }

    public static Glob ToGlob(this string self) => self.ToGlob(
        new GlobOptions { Evaluation = new EvaluationOptions { CaseInsensitive = true } }
    );
}
