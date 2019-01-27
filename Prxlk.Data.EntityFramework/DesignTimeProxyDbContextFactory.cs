using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Prxlk.Data.EntityFramework
{
    public class DesignTimeProxyDbContextFactory : IDesignTimeDbContextFactory<ProxyDbContext>
    {
        /// <inheritdoc />
        public ProxyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProxyDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProxiesDb;Integrated Security=true;");

            return new ProxyDbContext(optionsBuilder.Options);
        }
    }
}