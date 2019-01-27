using Microsoft.EntityFrameworkCore;
using Prxlk.Data.EntityFramework.Mappings;
using Prxlk.Domain.Models;

namespace Prxlk.Data.EntityFramework
{
    public class ProxyDbContext : DbContext
    {
        public DbSet<Proxy> Proxies { get; set; }

        public ProxyDbContext(string connectionString) 
            : base(CreateOptions(connectionString))
        { }
        
        public ProxyDbContext(DbContextOptions options)
            : base(options)
        { }
        
        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProxyMap());
            
            base.OnModelCreating(modelBuilder);
        }

        private static DbContextOptions CreateOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProxyDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProxiesDb;Integrated Security=true;");

            return optionsBuilder.Options;
        }
    }
}