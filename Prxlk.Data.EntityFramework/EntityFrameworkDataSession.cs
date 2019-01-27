using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Prxlk.Domain.DataAccess;

namespace Prxlk.Data.EntityFramework
{
    public class EntityFrameworkDataSession : IDataSession
    {
        private readonly ProxyDbContext _dbContext;

        public EntityFrameworkDataSession(ProxyDbContext dbContext, IsolationLevel? isolationLevel)
        {
            _dbContext = dbContext;

            if (isolationLevel.HasValue)
                _dbContext.Database.BeginTransaction(isolationLevel.Value);
            else
                _dbContext.Database.BeginTransaction();
        }

        /// <inheritdoc />
        public async Task CommitAsync(CancellationToken cancellation)
        {
            if (_dbContext.Database.CurrentTransaction != null)
            {
                await _dbContext.SaveChangesAsync(cancellation);
                _dbContext.Database.CommitTransaction();
            }
        }

        /// <inheritdoc />
        public Task RollbackAsync(CancellationToken cancellation)
        {
            if (_dbContext.Database.CurrentTransaction != null)
                _dbContext.Database.RollbackTransaction();
            
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_dbContext.Database.CurrentTransaction != null)
                _dbContext.Database.RollbackTransaction();
        }
    }
}