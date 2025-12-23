
using System.Security.Cryptography;
using System.Text;

namespace ExpenseManagement.Identity.Application.Common.Security
{
    public static class RefreshTokenHelper
    {
        public static string Generate()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

        public static string Hash(string token)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hash);
        }
    }
}
