using System.Threading.Tasks;
using DistributeCache.Models;

namespace DistributeCache.Data.Repositories
{
    public interface IDistributeCacheRepository
    {
         void UpSert(string key, string value);
         void Remove(string key);
         string Get(string key);
    }
}