using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace App.Infra.Integration.RabbitMq.Modules
{
    public class ConnectionConfiguration
    {
        public string Host { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClientProvidedName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FailReConnectRetryCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool AutomaticRecoveryEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan NetworkRecoveryInterval { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan ConsumerFailRetryInterval { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public LogLevel Level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ConnectionConfiguration()
        {
            Level = LogLevel.Information;
            FailReConnectRetryCount = 50;
            NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
            AutomaticRecoveryEnabled = true;
            ConsumerFailRetryInterval = TimeSpan.FromSeconds(1);
        }

        public static ConnectionConfiguration Parse(IConfiguration configuration)
           => configuration.GetSection(nameof(RabbitMq))
                           .Get<ConnectionConfiguration>();

        public override string ToString()
            => JsonConvert.SerializeObject(this);
    }
}
