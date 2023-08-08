using App.Application.Poc.Cache;
using App.Infra.Bootstrap.Attributes;
using App.Infra.Integration.Redis;
using App.Repositories;

namespace App.Application.Poc
{
    [Transient]
    public class Poc
    {    
        readonly RedisService _redis;

        public Poc(RedisService redis)
        {
            _redis = redis;
        }

        public void RedisCheck()
        {
            var k = _redis.Get<TesteCache>("1");
        }
    }
}
