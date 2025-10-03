using System;
using System.Security.Cryptography;
using System.Text;

namespace Online_Bookstore.Utils
{
    public static class CommonCrypto
    {
        public static string Sha256Hash(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
