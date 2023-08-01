using System;
using System.Reflection;

namespace App.Infra.Integration.Redis.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RedisAttribute : Attribute
    {
        public string Name { get; set; }

        public int Database { get; set; }

        /// <summary>
        /// Value in minutes
        /// </summary>
        public double Expiry { get; set; }

        public static RedisAttribute Parse(Type type)
           => type.GetTypeInfo()
                  .GetCustomAttribute<RedisAttribute>();
    }
}
