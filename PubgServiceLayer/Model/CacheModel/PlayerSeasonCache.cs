using Newtonsoft.Json;
using Pubg.Net;
using Pubg.Net.Models.Stats;
using System.Collections.Generic;

namespace PubgServiceLayer.Model
{
    public class PlayerSeasonCache
    {

        [JsonProperty]
        public PubgSeasonStats GameModeStats { get; set; }

        [JsonProperty("player")]
        public string PlayerId { get; set; }

        [JsonProperty("season")]
        public string SeasonId { get; set; }

        [JsonProperty("matchesSolo")]
        public IEnumerable<string> SoloMatchIds { get; set; }

        [JsonProperty("matchesSoloFPP")]
        public IEnumerable<string> SoloFPPMatchIds { get; set; }

        [JsonProperty("matchesDuo")]
        public IEnumerable<string> DuoMatchIds { get; set; }

        [JsonProperty("matchesDuoFPP")]
        public IEnumerable<string> DuoFPPMatchIds { get; set; }

        [JsonProperty("matchesSquad")]
        public IEnumerable<string> SquadMatchIds { get; set; }

        [JsonProperty("matchesSquadFPP")]
        public IEnumerable<string> SquadFPPMatchIds { get; set; }

        public static explicit operator PlayerSeasonCache(PubgPlayerSeason playerSeason)
        {
            return new PlayerSeasonCache
            {
                PlayerId = playerSeason.PlayerId,
                SeasonId = playerSeason.SeasonId,
                GameModeStats = playerSeason.GameModeStats,
                SoloMatchIds = playerSeason.SoloMatchIds,
                DuoMatchIds = playerSeason.DuoMatchIds,
                SquadMatchIds = playerSeason.SquadMatchIds,
                SoloFPPMatchIds = playerSeason.SoloFPPMatchIds,
                DuoFPPMatchIds = playerSeason.DuoFPPMatchIds,
                SquadFPPMatchIds = playerSeason.SquadFPPMatchIds
            };
        }

        public static implicit operator PubgPlayerSeason(PlayerSeasonCache playerSeason)
        {
            return new PubgPlayerSeason
            {
                PlayerId = playerSeason.PlayerId,
                SeasonId = playerSeason.SeasonId,
                GameModeStats = playerSeason.GameModeStats,
                SoloMatchIds = playerSeason.SoloMatchIds,
                DuoMatchIds = playerSeason.DuoMatchIds,
                SquadMatchIds = playerSeason.SquadMatchIds,
                SoloFPPMatchIds = playerSeason.SoloFPPMatchIds,
                DuoFPPMatchIds = playerSeason.DuoFPPMatchIds,
                SquadFPPMatchIds = playerSeason.SquadFPPMatchIds
            };
        }
    }
}
