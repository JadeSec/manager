using Microsoft.Extensions.Configuration;

namespace App.Infra.Integration.Hangfire.Models
{
    public class Option
    {
        public string Path { get; set; }

        public bool IsValid
            => !string.IsNullOrEmpty(Path);

        public static Option Parse(IConfiguration configuration)
            => configuration.GetSection(nameof(Hangfire))
                            .Get<Option>();
    }
}
