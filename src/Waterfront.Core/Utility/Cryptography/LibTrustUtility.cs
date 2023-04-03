using System.Security.Cryptography;
using SimpleBase;

namespace Waterfront.Core.Utility.Cryptography;

public static class LibTrustUtility
{
    private const char GROUP_DELIMITER = ':';
    private const int  GROUP_COUNT     = 12;
    private const int  CHARS_PER_GROUP = 4;
    private const int  SUB_HASH_LENGTH = 30;

    public static string GetKeyId(byte[] spki)
    {
        using SHA256 sha256  = SHA256.Create();
        byte[]       hash    = sha256.ComputeHash(spki);
        byte[]       subHash = hash[..SUB_HASH_LENGTH];
        string       base32  = Base32.Rfc4648.Encode(subHash);
        string[]     groups  = new string[GROUP_COUNT];
        for (int i = 0; i < GROUP_COUNT; i++)
        {
            int    startIndex = i * CHARS_PER_GROUP;
            int    endIndex   = startIndex + CHARS_PER_GROUP;
            string group      = base32[startIndex..endIndex];
            groups[i] = group;
        }

        string keyID = string.Join(GROUP_DELIMITER, groups);

        return keyID;
    }
}
