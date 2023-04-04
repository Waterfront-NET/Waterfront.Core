using System;

namespace Waterfront.Common.Acl;

#pragma warning disable CS8618

public class AclRefreshToken
{
    public string Value { get; }
    public DateTime CreatedAt { get; }
    public DateTime ExpiresAt { get; }

    /// <summary>
    /// <see cref="AclUser.Username"/> of the user that owns this token
    /// </summary>
    public string Owner { get; }

    public bool IsValid() => IsValid(DateTime.Now);
    public bool IsValid(DateTime atDate) => atDate < ExpiresAt;
}