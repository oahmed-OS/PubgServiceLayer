using System;
using Pubg.Net;

namespace PubgServiceLayer.Model
{
    public class PlayerCache
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ShardId { get; set; }

        public static explicit operator PlayerCache (PubgPlayer player)
        {
            return new PlayerCache
            {
                Id = player.Id,
                Name = player.Name,
                ShardId = player.ShardId
            };
        }

        public static implicit operator PubgPlayer (PlayerCache player)
        {
            return new PubgPlayer
            {
                Id = player.Id,
                Name = player.Name,
                ShardId = player.ShardId
            };
        }
    }
}
