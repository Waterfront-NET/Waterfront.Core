﻿namespace Waterfront.Common.Acl;

#pragma warning disable CS8618

public class AclPolicy
{
    /// <summary>
    /// Policy's identifier. Should not be empty or duplicated, or unexpected behaviour is to be expected
    /// </summary>
    public string Name { get; init; }
    /// <summary>
    /// A set of resource access rules which policy allows to match
    /// </summary>
    public IEnumerable<AclAccessRule> Access { get; init; }
}
