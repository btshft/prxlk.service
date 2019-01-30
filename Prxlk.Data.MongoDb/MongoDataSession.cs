using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Prxlk.Domain.DataAccess;

namespace Prxlk.Data.MongoDb
{
    public class MongoDataSession : IDataSession
    {
        private readonly IClientSessionHandle _session;

        public MongoDataSession(IMongoClientProvider provider)
        {
            _session = provider.GetClient().StartSession();
            _session.StartTransaction();
        }
        
        /// <inheritdoc />
        public Task CommitAsync(CancellationToken cancellation)
        {
            _session.CommitTransaction(cancellation);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task RollbackAsync(CancellationToken cancellation)
        {
            await _session.AbortTransactionAsync(cancellation);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _session.Dispose();
        }
    }
}