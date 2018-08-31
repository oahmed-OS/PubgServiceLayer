using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PubgServiceLayer.Api
{
    public class StatsHelper
    {
        private readonly PubgCacheManager pubgApi;

        public StatsHelper(PubgCacheManager cacheManager)
        {
            pubgApi = cacheManager;
        }


    }
}
