using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace App.Infra.CrossCutting.Helpers
{
    public static class CryptographyHelper
    {
        public static string Sha1ComputeHash(string value)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(value.Select(Convert.ToByte).ToArray());
                return BitConverter.ToString(hash)
                                   .Replace("-", string.Empty);
            }
        }

        public static string Base64UrlEncode(string value)
        {
            // Special "url-safe" base64 encode.
            var inputBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(inputBytes)
                          .Replace('+', '-') // replace URL unsafe characters with safe ones
                          .Replace('/', '_') // replace URL unsafe characters with safe ones
                          .Replace("=", ""); // no padding        
        }

        public static string Base64UrlDecode(string value)
        {
            string incoming = value.Replace('_', '/').Replace('-', '+');
            switch (value.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }
            byte[] bytes = Convert.FromBase64String(incoming);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
