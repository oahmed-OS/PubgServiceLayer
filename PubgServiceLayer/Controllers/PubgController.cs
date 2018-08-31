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

        // GET api/pubg/Player/{player}
        [HttpGet("Player/{playerName}")]
        public async Task<IActionResult> GetPlayer(string playerName)
        {
            if (String.IsNullOrEmpty(playerName))
                return NotFound();

            //var player = await new PubgApi(Configuration["PubgApiKey"])
            //    .GetPlayerByNameAsync(playerName);

            var player = await pubgApi.GetPlayerByNameAsync(playerName);

            if (player == null)
                return NotFound("Invalid Player Name");

            return Ok(player);
        }

        // GET api/pubg/seasons/{region}
        [HttpGet("Seasons/{region}")]
        public async Task<IActionResult> GetSeasons(PubgRegion region)
        {
            var seasons = await pubgApi.GetSeasonsAsync(region);

            if (seasons == null)
                return NotFound("Invalid Region");

            return Ok(seasons);
        }

        // GET api/pubg/playerstats/{player}
        [HttpGet("PlayerStats/{playerName}")]
        public async Task<IActionResult> GetPlayerStats(string playerName, string seasonId)
        {
            if (String.IsNullOrEmpty(playerName))
                return NotFound();

            var stats = await pubgApi.GetPlayerStatsAsync(playerName, seasonId);

            if (stats == null)
                return NotFound("Invalid Player Name or Season Id");

            return Ok(stats);
        }

    }
}