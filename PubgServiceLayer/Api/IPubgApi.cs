using Pubg.Net;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PubgServiceLayer.Api
{
    public interface IPubgApi
    {

        Task<PubgPlayer> GetPlayerByNameAsync(string playerName, PubgRegion region = PubgRegion.PCNorthAmerica);

        Task<IEnumerable<PubgSeason>> GetSeasonsAsync(PubgRegion region = PubgRegion.PCNorthAmerica);

        Task<PubgPlayerSeason> GetPlayerSeasonAsync(string playerId, string seasonId, PubgRegion region = PubgRegion.PCNorthAmerica);

        Task<PubgMatch> GetMatchAsync(string matchId, PubgRegion region = PubgRegion.PCNorthAmerica);
    }
}
