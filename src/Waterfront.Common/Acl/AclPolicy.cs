using System.Collections.Generic;

namespace Waterfront.Common.Acl;

#pragma warning disable CS8618

public class AclPolicy
{
    /// <summary>
    /// Policy's identifier. Should not be empty or duplicated, or unexpected behaviour is to be expected
    /// </summary>
    public string Name { get; init; }
    public IEnumerable<AclAccessRule> Access { get; init; }
}