using System;

namespace Waterfront.Common.Acl;

#pragma warning disable CS8618

public class AclRefreshToken
{
    public string Value { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime ExpiresAt { get; init; }

    /// <summary>
    /// <see cref="AclUser.Username"/> of the user that owns this token
    /// </summary>
    public string Owner { get; init; }

    /// <summary>
    /// Checks if this token is valid at the current moment. It is equivalent of calling <see cref="IsValid(DateTime)"/> with <see cref="DateTime"/>.<see cref="DateTime.Now"/>
    /// </summary>
    /// <returns></returns>
    public bool IsValid() => IsValid(DateTime.Now);

    /// <summary>
    /// Checks if this token will be valid at given <paramref name="dateTime"/>
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public bool IsValid(DateTime dateTime) => dateTime < ExpiresAt;
}
