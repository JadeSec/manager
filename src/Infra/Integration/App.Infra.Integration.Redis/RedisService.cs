using System;
using Newtonsoft.Json;
using StackExchange.Redis;
using App.Infra.Integration.Redis.Models;
using Microsoft.Extensions.Configuration;
using App.Infra.Bootstrap.Attributes;
using App.Infra.Bootstrap;
using App.Infra.Integration.Redis.Attributes;
using App.Infra.Integration.Redis.Extensions;
using App.Infra.Integration.Redis.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace App.Infra.Integration.Redis
{
    /// <summary>
    /// https://stackexchange.github.io/StackExchange.Redis/Basics
    /// </summary>
    [Singleton]
    public class RedisService : IService<RedisService>
    {
        readonly Option _option;
        private ConnectionMultiplexer _redis;

        readonly ILogger<RedisService> _logger;

        public RedisService(
            ILogger<RedisService> logger,
            IConfiguration configuration)
        {
            _logger = logger;

            _option = Option.Parse(configuration);
        }

        public bool Exist<T>(string key) where T : ICache
        {
            try
            {
                var option = _override(typeof(T));
                var instanse = _instance(option);

                return instanse.KeyExists(_realKey(option.Name, key));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return false;
            }
        }

        public T Get<T>(string key) where T : ICache
            => Get<T>(key, () => default);

        public bool Set<T>(string key, T data, DateTime? expiry = default) where T : ICache
        {
            try
            {
                var option = _override(typeof(T));
                var instanse = _instance(option);
                var datetime = _expiry(option, expiry);

                return instanse.StringSet(
                    key: _realKey(option.Name, key), 
                    value: _serialize(data, datetime), 
                    expiry: datetime.TimeOfDay
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return false;
            }
        }

        public bool Put<T>(string key, T data, DateTime? expiry = default) where T : ICache
        {
            try
            {
                var option = _override(typeof(T));
                var instanse = _instance(option);

                var value = Get<T>(key);
                if (value != null)
                {
                    if (Del<T>(key))
                    {
                        if (Set(key, data, expiry))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            return false;
        }

        public bool Del<T>(string key) where T : ICache
        {
            try
            {
                var option = _override(typeof(T));
                var instanse = _instance(option);

                return instanse.KeyDelete(_realKey(option.Name, key));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            return false;
        }

        public T Get<T>(string key, Func<T> set) where T: ICache
        {
            try
            {
                var option = _override(typeof(T));
                var instanse = _instance(option);

                var result = _deserialize<T>(instanse.StringGet(_realKey(option.Name, key)));
                if (DateTime.Now >= result.Expiration)
                    throw new RedisException("The cache expiry by payload expiration");

                return result.Data;
            }
            catch (RedisException e) when (e.Message.Contains("expiry"))
            {
                Del<T>(key);
                return default;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);

                return set();
            }
        }

        private IDatabase _instance(RedisAttribute option)
        {
            if (_connect())
            {
                return _redis.GetDatabase(_database(option));
            }

            throw new Exception("Error connecting to redis server.");
        }
       
        private string _realKey(string path, string key)
            => new RedisKey().Append(path.Clean())
                             .Append(":")
                             .Append(key);

        private Payload<T> _deserialize<T>(string str) where T : ICache
            => JsonConvert.DeserializeObject<Payload<T>>(str);

        private string _serialize<T>(T data, DateTime expiry)
            => JsonConvert.SerializeObject(new Payload<T>(data, expiry));

        private RedisAttribute _override(Type type)
            => RedisAttribute.Parse(type);

        private int _database(RedisAttribute optionOverride)
        {
            if (optionOverride.Database != default)
            {
                return optionOverride.Database;
            }
            else if (_option.Database != null)
            {
                return (int)_option.Database;
            }
            else if (_option.IsCluster)
            {
                return -1;
            }

            throw new RedisException("Redis database number not found in options!");
        }

        private DateTime _expiry(RedisAttribute optionOverride, DateTime? expiryOverride)
        {
            if(expiryOverride != null || expiryOverride != default)
            {
                return (DateTime)expiryOverride;
            }
            else if (optionOverride.Expiry != default)
            {
                return DateTime.Now
                               .AddMinutes(optionOverride.Expiry);
            }
            else if (_option.Expiry != null)
            {
                return DateTime.Now
                             .AddMinutes((double)_option.Expiry);
            }

            throw new RedisException("Redis expiry minutes not found in options!");
        }

        private bool _connect()
        {
            try
            {
                if (_redis == null)
                    _redis = ConnectionMultiplexer.Connect(_option.HostConnection);

                return true;
            }
            catch(Exception e)
            {
                _logger.LogCritical("Redis server not connected: " + e.Message);
            }

            return false;
        }
    }
}