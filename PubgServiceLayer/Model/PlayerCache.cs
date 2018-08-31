using Pubg.Net;

namespace PubgServiceLayer.Model
{
    public class PlayerCache
    {
        //Default Constructor
        public PlayerCache()
        {

        }

        public PlayerCache(PubgPlayer player)
        {
            Id = player.Id;

            Name = player.Name;

            ShardId = player.ShardId;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string ShardId { get; set; }
    }
}
