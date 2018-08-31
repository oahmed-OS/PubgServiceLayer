using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pubg.Net;
using PubgServiceLayer.Api;

namespace PubgServiceLayer.Controllers
{
    [Produces("application/json")]
    [Route("api/Pubg")]
    public class PubgController : Controller
    {
        private IConfiguration Configuration { get; set; }

        //Connect Configuration file
        public PubgController(IConfiguration config)
        {
            Configuration = config;
        }

        // GET api/pubg/Player/{player}
        [HttpGet("Player/{playerName}")]
        public async Task<IActionResult> GetPlayer(string playerName)
        {
            if (String.IsNullOrEmpty(playerName))
                return NotFound();

            var player = await new PubgApi(Configuration["PubgApiKey"])
                .GetPlayerByNameAsync(playerName);

            return Ok(player);
        }

        // GET api/pubg/seasons/{region}
        [HttpGet("Seasons/{region}")]
        public async Task<IActionResult> GetSeasons(PubgRegion region)
        {
            var seasons = await new PubgApi(Configuration["PubgApiKey"])
                .GetSeasons(region);
            return Ok(seasons);
        }

        // GET api/pubg/playerstats/{player}
        [HttpGet("PlayerStats/{playerName}")]
        public async Task<IActionResult> GetPlayerStats(string playerName)
        {
            if (String.IsNullOrEmpty(playerName))
                return NotFound();

            return Ok();
        }

    }
}