using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pubg.Net;

namespace PubgServiceLayer.Api
{
    public class PubgApi
    {
        public PubgApi(string apiKey)
        {
            PubgApiConfiguration.Configure(opt =>
            {
                opt.ApiKey = apiKey;
            });
        }

        public Task<IEnumerable<PubgPlayer>> GetPlayerByNameAsync(string playerName)
        {
            PubgPlayerService playerService = new PubgPlayerService();

            var request = new GetPubgPlayersRequest
            {
                PlayerNames = new string[] { playerName }
            };

            return playerService.GetPlayersAsync(PubgRegion.PCNorthAmerica,
                request);
        }
    }
}
