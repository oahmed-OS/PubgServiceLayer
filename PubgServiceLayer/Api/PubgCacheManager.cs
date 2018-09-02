using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pubg.Net;
using PubgServiceLayer.Model;
using PubgServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PubgServiceLayer.Api
{
    public class PubgCacheManager: IPubgApi
    {

        private readonly IRedisService _redisService;
        private readonly PubgApi pubgApi;


        public PubgCacheManager(string apiKey, IRedisService redisService)
        {
            pubgApi = new PubgApi(apiKey);
            _redisService = redisService;
        }

        public async Task<PubgPlayer> GetPlayerByNameAsync(string playerName, PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            return await GetAsync<PlayerCache>(playerName + region, async (t) =>
            {
                var retVal = await pubgApi.GetPlayerByNameAsync(playerName, region);
                return (PlayerCache)retVal;
            });
        }

        public async Task<PubgPlayerSeason> GetPlayerSeasonAsync(string playerId, string seasonId, PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            return await GetAsync<PlayerSeasonCache>(playerId + seasonId + region, async (t) =>
            {
                var retVal = await pubgApi.GetPlayerSeasonAsync(playerId, seasonId, region);
                return (PlayerSeasonCache)retVal;
            });
        }

        public async Task<PlayerStats> GetPlayerStatsAsync(string playerName, string seasonId, PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            return await GetAsync<PlayerStats>(playerName + seasonId + region, async (t) =>
            {
                return await new StatsHelper(this).FilterStats(playerName, seasonId, region);
            });
        }

        public async Task<IEnumerable<PubgSeason>> GetSeasonsAsync(PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            return await GetAsync<List<PubgSeason>>("seasons", async (t) =>
            {
                var retVal = await pubgApi.GetSeasonsAsync(region);
                return retVal.ToList();
            });
        }

        //TODO: Refactor cache so we only store needed info, not entire match object
        public async Task<PubgMatch> GetMatchAsync(string matchId, PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            return await GetAsync<PubgMatch>(matchId + region, async (t) =>
            {
                return await pubgApi.GetMatchAsync(matchId, region);
            });
        }

        public async Task<T> GetAsync<T>(string key, Func<T, Task<T>> action) where T : class, new()
        {
           var result = new T();
            _redisService.Remove(key);
            var cacheResult = await _redisService.GetAsync(key);

            if (String.IsNullOrEmpty(cacheResult))
            {
                //Cache data not found or expired
                result = await action(result);

                if (result != null)
                {
                    _redisService.Set(key, JsonConvert.SerializeObject(result));
                }

            }
            else
            {
                //Pull from Cache
                result = JsonConvert.DeserializeObject<T>(cacheResult);
            }
            return result;
        }
    }
}
