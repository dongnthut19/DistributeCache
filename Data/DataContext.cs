using DistributeCache.Models;
using Microsoft.EntityFrameworkCore;

namespace DistributeCache.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {   
        }

        public DbSet<DistributeCacheModel> DistributeCacheModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}