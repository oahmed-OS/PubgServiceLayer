using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace PubgServiceLayer.Services
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _cache;
        private readonly TimeSpan ValidityPeriod;
        public RedisService(IDatabase cache)
        {
            _cache = cache;

            //Only store cache for 10 minutes before refreshing
            ValidityPeriod = TimeSpan.FromMinutes(10);
        }

        public string Get(string key)
            =>  _cache.StringGet(key);

        public bool Set(string key, string obj)
            => _cache.StringSet(key, obj, ValidityPeriod);

        public bool Remove(string key)
            => _cache.KeyDelete(key);

        public async Task<string> GetAsync(string key)
            => await _cache.StringGetAsync(key);

        public async Task<bool> SetAsync(string key, string obj)
            => await _cache.StringSetAsync(key, obj, ValidityPeriod);
    }
}
