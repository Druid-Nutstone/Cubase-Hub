
namespace Cubase.Hub.Services.Distributers.SoundCloud
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class PkceHelper
    {
        public static string GenerateCodeVerifier()
        {
            var bytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            return Base64UrlEncode(bytes);
        }

        public static string GenerateCodeChallenge(string codeVerifier)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] challengeBytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(codeVerifier));
                return Base64UrlEncode(challengeBytes);
            }
        }

        private static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }
    }
}
