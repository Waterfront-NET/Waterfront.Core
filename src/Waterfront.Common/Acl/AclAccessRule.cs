﻿using System.Diagnostics;

namespace Waterfront.Common.Acl;

#pragma warning disable CS8618

/// <summary>
/// Represents a resource access rule
/// </summary>
[DebuggerDisplay("{ToString()}")]
public class AclAccessRule
{
    /// <summary>
    /// The resource type
    /// </summary>
    public AclResourceType Type { get; init; }

    /// <summary>
    /// Name of the resource, may include glob pattern characters
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Defines a set of actions user is allowed to perform on a resource
    /// </summary>
    public IEnumerable<AclResourceAction> Actions { get; init; }

    public override string ToString() =>
        $"AclAccessRule {{ Type={Type:G}; Name=\"{Name}\"; Actions=[{Actions.Select(action => action.ToString("G"))}] }}";
}
