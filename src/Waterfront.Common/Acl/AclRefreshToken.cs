using System;

namespace Waterfront.Common.Acl;

public class AclRefreshToken
{
    public string Value { get; }
    public DateTime CreatedAt { get; }
    public DateTime ExpiresAt { get; }

    /// <summary>
    /// <see cref="AclUser"/> that owns this token
    /// </summary>
    public string Owner { get; }

    public bool IsValid() => IsValid(DateTime.Now);
    public bool IsValid(DateTime atDate) => atDate < ExpiresAt;
}