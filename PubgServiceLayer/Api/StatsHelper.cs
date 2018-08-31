using Pubg.Net;
using Pubg.Net.Models.Stats;
using PubgServiceLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PubgServiceLayer.Api
{
    public static class StatsHelper
    {
        public static async Task<PlayerStats> FilterStats(PubgCacheManager pubgApi, 
            string playerName, string seasonId, PubgRegion region = PubgRegion.PCNorthAmerica)
        {
            //Wrapping in try catch in case player name or season id is incorrect
            try
            {

                PlayerStats playerStats = new PlayerStats();

                //Get Player
                var player = await pubgApi.GetPlayerByNameAsync(playerName)
                    .ConfigureAwait(false);

                //Get Player Season
                var playerSeason = await pubgApi.GetPlayerSeasonAsync(player.Id,
                    seasonId, region).ConfigureAwait(false);

                //Filter Matches
                var validMatches = await FilterMatches(LoadMatches(playerSeason))
                    .ConfigureAwait(false);

                //Compute Stats
                return BuildStats(playerName, playerSeason.GameModeStats,
                    validMatches);

            }
            catch (NullReferenceException e) { }


            return null;
        }

        private static List<string> LoadMatches(PubgPlayerSeason playerSeason)
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

        private static Task<int> FilterMatches(IEnumerable<string> matchIds)
        {

            return null;
        }

        private static PlayerStats BuildStats(string playerName, PubgSeasonStats stats, int validMatches)
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

        private static PubgModeStats PullModeStats(PubgGameModeStats stats, int validMatches)
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
