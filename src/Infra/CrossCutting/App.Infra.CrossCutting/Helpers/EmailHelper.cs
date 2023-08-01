using System.Net.Mail;

namespace App.Infra.CrossCutting.Helpers
{
    public static class EmailHelper
    {
        public static string Normalize(string email)
            => email.Trim()
                    .ToLower();

        public static bool IsValid(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
