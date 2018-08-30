using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PubgServiceLayer.Controllers
{
    [Produces("application/json")]
    [Route("api/Pubg")]
    public class PubgController : Controller
    {
        // GET api/pubg
        [HttpGet]
        public IEnumerable<string> GetPlayer(string player)
        {
            return new string[] { "Pubg", "Controller" };
        }
    }
}