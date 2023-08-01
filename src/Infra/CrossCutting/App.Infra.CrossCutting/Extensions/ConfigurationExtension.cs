using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace App.Infra.CrossCutting.Extensions
{
    public static class ConfigurationExtension
    {
        public static IConfiguration UpdateFromEnvironment(this IConfiguration configuration)
        {
            foreach (var item in configuration.AsEnumerable())
            {
                string envKey = item.Key.ToUpper().Replace(":", "_");

                configuration[item.Key] = Environment.GetEnvironmentVariable(envKey) ?? configuration[item.Key];               
            }

            return configuration;
        }
    }
}
