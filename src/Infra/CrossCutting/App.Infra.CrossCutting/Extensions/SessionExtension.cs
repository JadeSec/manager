using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace App.Infra.CrossCutting.Extensions
{
    public static class SessionExtension
    {
        public static async Task SetObjectAsync<T>(this ISession session, string key, T value)
        {
            await session.LoadAsync();
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static async Task<T> GetObjectAsync<T>(this ISession session, string key)
        {
            await session.LoadAsync();
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static async Task SetAsync(this ISession session, string key, string value)
        {
            await session.LoadAsync();
            session.SetString(key, value);
        }

        public static bool Exist(this ISession session, string key)
        {
            return (session.GetString(key) == null) ? false : true;
        }

        public static async Task<T> GetUniqueAsync<T>(this ISession session, string key)
        {
            await session.LoadAsync();

            var value = session.GetString(key);
            session.Remove(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
