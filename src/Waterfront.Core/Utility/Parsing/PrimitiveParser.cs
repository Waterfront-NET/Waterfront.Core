using System;

namespace Waterfront.Core.Utility.Parsing;

public static class PrimitiveParser
{
    public static bool ParseBoolean(string? input) => input switch {
                                                          "true" or "True" or "TRUE" or "1" or "yes"
                                                          or "Yes" or "YES" or "Y" or "y" => true,
                                                          "false" or "False" or "FALSE" or "0"
                                                          or "no" or "No" or "NO" or "N" or "n"
                                                          or "" or null => false,
                                                          _ => throw new ArgumentException("Invalid input format")
                                                      };
}
