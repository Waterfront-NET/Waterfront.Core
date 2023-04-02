namespace Waterfront.Common.Acl;

public class AclUser
{
    /// <summary>
    /// User name
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// List of policies user is authorized to use
    /// </summary>
    public IEnumerable<string> Acl { get; }
}