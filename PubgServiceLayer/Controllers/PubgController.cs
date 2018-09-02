using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pubg.Net;
using PubgServiceLayer.Api;
using PubgServiceLayer.Services;

namespace PubgServiceLayer.Controllers
{
    [Produces("application/json")]
    [Route("api/Pubg")]
    public class PubgController : Controller
    {
        private IConfiguration Configuration { get; set; }

        private readonly PubgCacheManager pubgApi;

        //Connect Configuration file
        public PubgController(IConfiguration config, IRedisService redisService)
        {
            Configuration = config;
            pubgApi = new PubgCacheManager(Configuration["PubgApiKey"], 
                redisService);
        }

        // GET api/pubg/player/{player}/server/{region}
        [HttpGet("player/{playerName}/server/{region}")]
        public async Task<IActionResult> GetPlayer(string playerName, PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            if (String.IsNullOrEmpty(playerName))
                return NotFound();

            try
            {
                var player = await pubgApi.GetPlayerByNameAsync(playerName, region);
                return Ok(player);
            }
            catch (Exception e)
            {
                return NotFound("Player Not Found");
            }

            
        }

        // GET api/pubg/seasons/{region}
        [HttpGet("seasons/{region}")]
        public async Task<IActionResult> GetSeasons(PubgRegion region)
        {
            try
            {
                var seasons = await pubgApi.GetSeasonsAsync(region);
                return Ok(seasons);
            }
            catch (Exception e)
            {
                return NotFound("Seasons Not Found For Region");

            }            
        }

        // GET api/pubg/playerstats/{player}/season/{seasonId}/region/{region}
        [HttpGet("playerstats/{playerName}/season/{seasonId}/region/{region}")]
        public async Task<IActionResult> GetPlayerStats(string playerName, string seasonId, PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            if (String.IsNullOrEmpty(playerName))
                return NotFound();
            try
            {
                var stats = await pubgApi.GetPlayerStatsAsync(playerName, seasonId, region);

                if(stats == null)
                    return NotFound("Season Data Not Found");

                return Ok(stats);
            }catch(Exception e)
            {
                return NotFound("Invalid Player Name or Season Id");
            }
            
        }

    }
}