using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace App.Infra.CrossCutting.Extensions
{
    public static class StringExtension
    {
        public const string RANDOM_NUMBERS = "1234567890";
        public const string RANDOM_LETTERS_FRIENDLY = "ABCDEFGHJKMNPQRSTUVXWYZ";
        public const string RANDOM_LETTERS_NUMBERS_ESPECIAL = "1A2B3C4D5E6F7G8H9J!K@M#N$P%Q&R*S(T)U_V-X+W=YZ";

        public static string EncodeBase64(this string value)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(valueBytes);
        }

        public static string DecodeBase64(this string value)
        {
            var valueBytes = System.Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(valueBytes);
        }

        public static string CapitalizeFirstLetter(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;
            if (s.Length == 1)
                return s.ToUpper();
            return s.Remove(1).ToUpper() + s.Substring(1);
        }

        public static string Random(this string str, int length)
        {
            str = (string.IsNullOrEmpty(str)) ? RANDOM_LETTERS_FRIENDLY : str;

            if (length < 0)
                throw new ArgumentException("length must not be negative", "length");
            if (length > int.MaxValue / 8)
                throw new ArgumentException("length is too big", "length");
            if (str == null)
                throw new ArgumentNullException("characterSet");
            var characterArray = str.Distinct().ToArray();
            if (characterArray.Length == 0)
                throw new ArgumentException("characterSet must not be empty", "characterSet");

            var bytes = new byte[length * 8];
            new RNGCryptoServiceProvider().GetBytes(bytes);
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                ulong value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }
            return new string(result);
        }
    }
}