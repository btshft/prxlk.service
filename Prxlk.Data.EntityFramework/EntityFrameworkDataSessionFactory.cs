using System.Data;
using Prxlk.Domain.DataAccess;

namespace Prxlk.Data.EntityFramework
{
    public class EntityFrameworkDataSessionFactory : IDataSessionFactory
    {
        private readonly ProxyDbContext _dbContext;

        public EntityFrameworkDataSessionFactory(ProxyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public IDataSession CreateSession()
        {
            return new EntityFrameworkDataSession(_dbContext, isolationLevel: null);
        }

        /// <inheritdoc />
        public IDataSession CreateSession(IsolationLevel isolationLevel)
        {
            return new EntityFrameworkDataSession(_dbContext, isolationLevel);
        }
    }
}