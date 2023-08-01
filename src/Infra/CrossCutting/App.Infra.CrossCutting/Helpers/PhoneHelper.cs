using System.Text.RegularExpressions;

namespace App.Infra.CrossCutting.Helpers
{
    public static class PhoneHelper
    {
        public static string Clean(string cell)
        {
            Regex regex = new Regex("\\W*");
            return regex.Replace(cell, string.Empty);
        }

        public static string Normalize(string cell)
            => Clean(cell).Insert(0, "+");

        public static bool IsValid(string number)
        {
            var clean = Clean(number);
            return (clean.Length >= 3 && clean.Length <= 15);
        }         
    }
}
