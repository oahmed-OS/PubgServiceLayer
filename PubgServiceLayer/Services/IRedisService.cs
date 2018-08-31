using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PubgServiceLayer.Services
{
    public interface IRedisService
    {

        string Get(string key);

        bool Set(string key, string obj);

        bool Remove(string key);

        Task<string> GetAsync(string key);

        Task<bool> SetAsync(string key, string obj);
    }
}
