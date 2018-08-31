using System;
using System.Collections.Generic;
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

        public PubgPlayer GetPlayerByName(string playerName, PubgRegion region = PubgRegion.PCNorthAmerica)
        {

            PubgPlayerService playerService = new PubgPlayerService();

            var request = new GetPubgPlayersRequest
            {
                PlayerNames = new string[] { playerName }
            };

            List<PubgPlayer> response = (List<PubgPlayer>)playerService.GetPlayers(region,
                request);

            if (response.Count > 0)
                return response[0];

            return null;
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

        public Task<IEnumerable<PubgSeason>> GetSeasonsAsync(PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            return new PubgSeasonService().GetSeasonsAsync(region);
        }
    }
}
