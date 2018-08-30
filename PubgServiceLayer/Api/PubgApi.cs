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

        public Task<IEnumerable<PubgPlayer>> GetPlayerByNameAsync(string playerName, PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            PubgPlayerService playerService = new PubgPlayerService();

            var request = new GetPubgPlayersRequest
            {
                PlayerNames = new string[] { playerName }
            };

            return playerService.GetPlayersAsync(region,
                request);
        }

        public Task<IEnumerable<PubgSeason>> GetSeasons(PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            return new PubgSeasonService().GetSeasonsAsync(region);
        }
    }
}
