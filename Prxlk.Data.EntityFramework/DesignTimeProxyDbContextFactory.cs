using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Prxlk.Data.EntityFramework
{
    public class DesignTimeProxyDbContextFactory : IDesignTimeDbContextFactory<ProxyDbContext>
    {
        /// <inheritdoc />
        public ProxyDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            var optionsBuilder = new DbContextOptionsBuilder<ProxyDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new ProxyDbContext(optionsBuilder.Options);
        }
    }
}