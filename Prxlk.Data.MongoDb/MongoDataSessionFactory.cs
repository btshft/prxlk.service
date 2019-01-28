using System.Data;
using Prxlk.Domain.DataAccess;

namespace Prxlk.Data.MongoDb
{
    public class MongoDataSessionFactory : IDataSessionFactory
    {
        private readonly IMongoClientProvider _clientProvider;

        public MongoDataSessionFactory(IMongoClientProvider clientProvider)
        {
            _clientProvider = clientProvider;
        }

        /// <inheritdoc />
        public IDataSession CreateSession()
        {
            return new MongoDataSession(_clientProvider);
        }

        /// <inheritdoc />
        public IDataSession CreateSession(IsolationLevel isolationLevel)
        {
            return new MongoDataSession(_clientProvider);
        }
    }
}