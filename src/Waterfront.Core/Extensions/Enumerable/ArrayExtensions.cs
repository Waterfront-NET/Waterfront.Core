using System.Diagnostics.CodeAnalysis;

namespace Waterfront.Core.Extensions.Enumerable;

public static class ArrayExtensions
{
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this T[]? array)
    {
        return array == null || array.Length == 0;
    }
}
