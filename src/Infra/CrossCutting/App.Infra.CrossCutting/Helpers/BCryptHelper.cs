using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infra.CrossCutting.Helpers
{
    public class BCryptHelper
    {
        public static string Hash(string value, int workFactor = 5)
            => BCrypt.Net.BCrypt.HashPassword(value, workFactor, BCrypt.Net.SaltRevision.Revision2B);

        public static bool Verify(string value, string hash)
            => BCrypt.Net.BCrypt.Verify(value, hash);
    }
}