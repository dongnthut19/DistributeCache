using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributeCache.Models;
using Microsoft.EntityFrameworkCore;

namespace DistributeCache.Data.Repositories
{
    public class DistributeCacheRepository : IDistributeCacheRepository
    {
        private readonly DataContext _context;
        private static readonly object LockObj = new object();
        private static readonly Dictionary<string, DistributeCacheModel> Cache = new Dictionary<string, DistributeCacheModel>();
        public TimeSpan Timeout { get; set; }
        const int second = 15*60; // 15 minutes
        
        public DistributeCacheRepository(DataContext context)
        {
            _context = context;
            Timeout = TimeSpan.FromSeconds(second);
        }

        public void UpSert(string key, string value)
        {
            lock (LockObj)
            {
                if (Cache.ContainsKey(key))
                {
                    Update(key, value);
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        public void Remove(string key)
        {
            lock (LockObj)
            {
                var item = _context.DistributeCacheModels.Where(x => x.Key.ToLower().Contains(key.ToLower())).FirstOrDefault();
                if (item != null) {
                    _context.DistributeCacheModels.Remove(item);
                }
            }
        }

        public string Get(string key)
        {
            lock (LockObj)
            {
                if (Cache.ContainsKey(key))
                {
                    if (Cache[key].CreatedDate.Add(Timeout) < DateTime.Now)
                    {
                        return null;
                    }

                    var byteValue = Cache[key].Value;
                    return Encoding.UTF8.GetString(byteValue);
                }
                else
                {
                    var item = _context.DistributeCacheModels.Where(x => x.Key.ToLower().Contains(key.ToLower())).FirstOrDefault();
                    if (item == null) {
                        return null;
                    }

                    if (item.CreatedDate.Add(Timeout) < DateTime.Now) 
                    {
                        return null;
                    }
                    
                    var byteValue = item.Value;
                    return Encoding.UTF8.GetString(byteValue);
                }
            }
        }

        private void Add(string key, string value)
        {
            byte[] encodedValue = Encoding.UTF8.GetBytes(value);
            Cache[key] = new DistributeCacheModel {
                Key = key,
                Value = encodedValue,
                CreatedDate = DateTime.Now, 
                SlidingExpirationInSecond = second
            };

            _context.Add(Cache[key]);
            _context.SaveChanges();
        }

        private void Update(string key, string value)
        {
            var item = _context.DistributeCacheModels.Where(x => x.Key.ToLower().Contains(key.ToLower())).FirstOrDefault();
            item.Value = Encoding.UTF8.GetBytes(value);
            
            _context.Update(item);
            _context.SaveChanges();

            Cache[key] = new DistributeCacheModel {
                Key = key,
                Value = item.Value,
                CreatedDate = DateTime.Now, 
                SlidingExpirationInSecond = second
            };
        }
    }
}