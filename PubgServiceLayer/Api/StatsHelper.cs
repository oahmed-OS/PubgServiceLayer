using Pubg.Net;
using Pubg.Net.Models.Stats;
using PubgServiceLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PubgServiceLayer.Api
{
    public class StatsHelper
    {
        private readonly PubgCacheManager pubgApi;
        private string playerName;
        private string seasonId;
        private PubgRegion region;
        private string playerId;
        private const double ValidMatchTime = 300;

        public StatsHelper(PubgCacheManager pubgApi)
        {
            this.pubgApi = pubgApi;
        }

        public async Task<PlayerStats> FilterStats(string playerName, string seasonId,
            PubgRegion region = PubgRegion.PCNorthAmerica)
        {

            //Wrapping in try catch in case player name or season id is incorrect
            try
            {

                PlayerStats playerStats = new PlayerStats();

                //Populate object properties
                this.playerName = playerName;
                this.seasonId = seasonId;
                this.region = region;

                //Get Player
                var player = await pubgApi.GetPlayerByNameAsync(playerName)
                    .ConfigureAwait(false);

                playerId = player.Id;

                //Get Player Season
                var playerSeason = await pubgApi.GetPlayerSeasonAsync(playerId,
                    seasonId, region).ConfigureAwait(false);

                //Filter Matches
                var validMatches = await FilterMatches(LoadMatches(playerSeason))
                    .ConfigureAwait(false);

                //Compute Stats
                return BuildStats(playerSeason.GameModeStats,
                    validMatches);

            }
            catch (NullReferenceException e) { }


            return null;
        }

        private List<string> LoadMatches(PubgPlayerSeason playerSeason)
        {
            List<string> matches = new List<string>();

            matches.AddRange(playerSeason.SoloMatchIds);
            matches.AddRange(playerSeason.SoloFPPMatchIds);

            matches.AddRange(playerSeason.DuoMatchIds);
            matches.AddRange(playerSeason.DuoFPPMatchIds);

            matches.AddRange(playerSeason.SquadMatchIds);
            matches.AddRange(playerSeason.SquadFPPMatchIds);

            return matches;
        }

        //TODO: Complete Method to return number of valid matches
        private async Task<int> FilterMatches(List<string> matchIds)
        {
            List<Task<bool>> matchTasks = new List<Task<bool>>();
            for (int i = 0; i < matchIds.Count(); i++)
                matchTasks.Add(ValidateMatch(matchIds[i]));

            var result = await Task.WhenAll(matchTasks).ConfigureAwait(false);

            return result.Select(x => x)
                .Where(x => x == true)
                .Count(); ;
        }

        private async Task<bool> ValidateMatch(string matchId)
        {
            var match = await pubgApi.GetMatchAsync(matchId, region).ConfigureAwait(false);

            var player = match.Rosters.SelectMany(c => c.Participants)
                .Where(x => x.Stats.PlayerId == playerId).First();

            if (player.Stats.TimeSurvived > ValidMatchTime || player.Stats.Kills > 0)
                return true;

            return false;
        }

        private PlayerStats BuildStats(PubgSeasonStats stats, int validMatches)
        {
            PlayerStats playerStats = new PlayerStats();
            playerStats.PlayerName = playerName;

            playerStats.SoloTPP = PullModeStats(stats.Solo, validMatches);
            playerStats.SoloFPP = PullModeStats(stats.SoloFPP, validMatches);

            playerStats.DuoTPP = PullModeStats(stats.Duo, validMatches);
            playerStats.DuoFPP = PullModeStats(stats.DuoFPP, validMatches);

            playerStats.SquadTPP = PullModeStats(stats.Squad, validMatches);
            playerStats.SquadFPP = PullModeStats(stats.SquadFPP, validMatches);

            return playerStats;
        }

        private PubgModeStats PullModeStats(PubgGameModeStats stats, int validMatches)
        {
            return new PubgModeStats
            {
                KillDeath = stats.Kills / (float)validMatches,
                AverageDamage = (int)Math.Round(stats.DamageDealt / validMatches),
                WinRatio = stats.Wins / (float)validMatches
            };
        }
    }
}
