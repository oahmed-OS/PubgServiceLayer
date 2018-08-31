using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pubg.Net;
using PubgServiceLayer.Model;
using PubgServiceLayer.Services;
using System;
using System.Threading.Tasks;

namespace PubgServiceLayer.Api
{
    public class PubgCacheManager
    {

        private readonly IRedisService _redisService;
        private readonly PubgApi pubgApi;


        public PubgCacheManager(string apiKey, IRedisService redisService)
        {
            pubgApi = new PubgApi(apiKey);
            _redisService = redisService;
        }

        public Task<PlayerCache> GetPlayerByNameAsync(string playerName, PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            return Task.Run<PlayerCache>(() =>
            {
                PlayerCache result = new PlayerCache();
                var cacheResult = _redisService.Get(playerName + region);

                //Call Api if no cach for player
                if(String.IsNullOrEmpty(cacheResult))
                {
                    result = new PlayerCache(pubgApi.GetPlayerByName(playerName, region));
                    if(!String.IsNullOrEmpty(result.Name))
                    {
                        _redisService.Set(playerName + region, JsonConvert.SerializeObject(result));
                    }
                    
                }
                else
                {
                    //Pull from Cache
                    JToken token = JToken.Parse(cacheResult);
                    result = JsonConvert.DeserializeObject<PlayerCache>(cacheResult);
                }
                return result;
            });
        }
    }
}
