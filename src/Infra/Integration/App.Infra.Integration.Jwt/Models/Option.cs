using System.Text;
using Microsoft.Extensions.Configuration;

namespace App.Infra.Integration.Jwt.Models
{
    internal class Option
    {
        public string Audience { get; set; }

        public string Issuer { get; set; }

        public string Secret { get; set; }

        public byte[] SecretKey 
             => Encoding.ASCII.GetBytes(Secret);

        public static Option Parse(IConfiguration configuration)
            => configuration.GetSection(nameof(Jwt))
                            .Get<Option>();
    }
}
