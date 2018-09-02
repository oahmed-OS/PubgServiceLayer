using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pubg.Net;

namespace PubgServiceLayer.Api
{
    public class PubgApi : IPubgApi
    {

        public PubgApi(string apiKey)
        {
            PubgApiConfiguration.Configure(opt =>
            {
                opt.ApiKey = apiKey;
            });

        }

        public async Task<PubgPlayer> GetPlayerByNameAsync(string playerName, PubgRegion region = PubgRegion.PCNorthAmerica)
        {

            PubgPlayerService playerService = new PubgPlayerService();

            var request = new GetPubgPlayersRequest
            {
                PlayerNames = new string[] { playerName }
            };
            var response = await playerService.GetPlayersAsync(region,
                request);

            return response.First();
        }

        public Task<IEnumerable<PubgSeason>> GetSeasonsAsync(PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            return new PubgSeasonService()
                .GetSeasonsAsync(region);
        }

        public Task<PubgPlayerSeason> GetPlayerSeasonAsync(string playerId, string seasonId, PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            return new PubgPlayerService()
                .GetPlayerSeasonAsync(region, playerId, seasonId);
        }

        public Task<PubgMatch> GetMatchAsync(string matchId, PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            return new PubgMatchService().GetMatchAsync(region, matchId);
        }
    }
}
