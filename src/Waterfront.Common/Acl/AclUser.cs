using System.Collections.Generic;

namespace Waterfront.Common.Acl;

#pragma warning disable CS8618

public class AclUser
{
    /// <summary>
    /// User's identifier. Should not be empty or duplicated, or unexpected behaviour is to be expected
    /// </summary>
    public string Username { get; init; }

    /// <summary>
    /// List of policies user is authorized to use
    /// </summary>
    public IEnumerable<string> Acl { get; init; }
}
