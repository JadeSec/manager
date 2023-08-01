using System;

namespace App.Infra.Integration.Redis.Models
{
    internal class Payload<T>
    {
        public T Data { get; set; }

        public DateTime Expiration { get; set; }

        public Payload(T data, DateTime expiration)
        {
            Data = data;
            Expiration = expiration;
        }
    }
}
