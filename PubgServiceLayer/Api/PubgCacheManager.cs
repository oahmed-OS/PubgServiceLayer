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

        //Version 1
        //public Task<PlayerCache> GetPlayerByNameAsync(string playerName, PubgRegion region = PubgRegion.PCNorthAmerica)
        //{
        //    return Task.Run<PlayerCache>(() =>
        //    {
        //        PlayerCache result = new PlayerCache();
        //        var cacheResult = _redisService.Get(playerName + region);

        //        //Call Api if no cach for player
        //        if(String.IsNullOrEmpty(cacheResult))
        //        {
        //            result = new PlayerCache(pubgApi.GetPlayerByName(playerName, region));
        //            if(!String.IsNullOrEmpty(result.Name))
        //            {
        //                _redisService.Set(playerName + region, JsonConvert.SerializeObject(result));
        //            }

        //        }
        //        else
        //        {
        //            //Pull from Cache
        //            result = JsonConvert.DeserializeObject<PlayerCache>(cacheResult);
        //        }
        //        return result;
        //    });
        //}

        //Version 2
        public async Task<PubgPlayer> GetPlayerByNameAsync(string playerName, PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            PubgPlayer result = new PubgPlayer();
            var cacheResult = await _redisService.GetAsync(playerName + region).ConfigureAwait(false);

            if (String.IsNullOrEmpty(cacheResult))
            {
                //Cache data not found or expired
                result = await pubgApi.GetPlayerByNameAsync(playerName, region).ConfigureAwait(false);

                if (result != null)
                {
                    _redisService.Set(playerName + region, JsonConvert.SerializeObject(new PlayerCache(result)));
                }

            }
            else
            {
                //Pull from Cache
                result = JsonConvert.DeserializeObject<PubgPlayer>(cacheResult);
            }
            return result;
        }

        public async Task<PubgPlayerSeason> GetPlayerSeasonAsync(string playerId, string seasonId, PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            PubgPlayerSeason result = new PubgPlayerSeason();
            var cacheResult = await _redisService.GetAsync(playerId + seasonId + region).ConfigureAwait(false);

            if (String.IsNullOrEmpty(cacheResult))
            {
                //Cache data not found or expired
                result = await pubgApi.GetPlayerSeasonAsync(playerId, seasonId, region).ConfigureAwait(false);

                if (result != null)
                {
                    _redisService.Set(playerId + seasonId + region, JsonConvert.SerializeObject(result));
                }

            }
            else
            {
                //Pull from Cache
                result = JsonConvert.DeserializeObject<PubgPlayerSeason>(cacheResult);
            }
            return result;
        }

        public async Task<PlayerStats> GetPlayerStatsAsync(string playerName, string seasonId, PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            PlayerStats result = new PlayerStats();
            var cacheResult = await _redisService.GetAsync(playerName + seasonId + region).ConfigureAwait(false);

            if (String.IsNullOrEmpty(cacheResult))
            {
                //Cache data not found or expired
                result = await StatsHelper.FilterStats(this, playerName, seasonId, region).ConfigureAwait(false);

                if (result != null)
                {
                    _redisService.Set(playerName + seasonId + region, JsonConvert.SerializeObject(result));
                }

            }
            else
            {
                //Pull from Cache
                result = JsonConvert.DeserializeObject<PlayerStats>(cacheResult);
            }
            return result;
        }

        public async Task<IEnumerable<PubgSeason>> GetSeasonsAsync(PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            IEnumerable<PubgSeason> result = new List<PubgSeason>();
            var cacheResult = await _redisService.GetAsync("seasons").ConfigureAwait(false);

            if (String.IsNullOrEmpty(cacheResult))
            {
                //Cache data not found or expired
                result = await pubgApi.GetSeasonsAsync(region).ConfigureAwait(false);

                if (result != null && result.Count() > 0)
                {
                    _redisService.Set("seasons", JsonConvert.SerializeObject(result));
                }

            }
            else
            {
                //Pull from Cache
                result = JsonConvert.DeserializeObject<IEnumerable<PubgSeason>>(cacheResult);
            }
            return result;
        }

        //TODO: Refactor cache so we only store needed info, not entire match object
        public async Task<PubgMatch> GetMatchAsync(string matchId, PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            PubgMatch result = new PubgMatch();
            var cacheResult = await _redisService.GetAsync(matchId + region).ConfigureAwait(false);

            if (String.IsNullOrEmpty(cacheResult))
            {
                //Cache data not found or expired
                result = await pubgApi.GetMatchAsync(matchId, region).ConfigureAwait(false);

                if (result != null)
                {
                    _redisService.Set(matchId + region, JsonConvert.SerializeObject(result));
                }

            }
            else
            {
                //Pull from Cache
                result = JsonConvert.DeserializeObject<PubgMatch>(cacheResult);
            }
            return result;
        }
    }
}
