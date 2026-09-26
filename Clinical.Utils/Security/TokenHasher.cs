using System.Security.Cryptography;
using System.Text;

namespace Clinical.Utils.Security
{
    /// <summary>
    /// Hashes high-entropy tokens (refresh / reset) with SHA-256 before persisting them,
    /// so a database leak does not expose usable tokens. The raw token stays with the client.
    /// SHA-256 (not BCrypt) is correct here because the input is already random and high-entropy.
    /// </summary>
    public static class TokenHasher
    {
        public static string Hash(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}
