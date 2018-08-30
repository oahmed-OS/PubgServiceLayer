using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        // GET api/pubg/player
        [HttpGet("{player}")]
        public Task<IEnumerable<PubgPlayer>> GetPlayer(string player)
        {
            return new PubgApi(Configuration["PubgApiKey"])
                .GetPlayerByNameAsync(player);
        }
    }
}