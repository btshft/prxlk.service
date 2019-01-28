using System;
using MongoDB.Driver;

namespace Prxlk.Data.MongoDb
{
    public class MongoInternalsProvider : IMongoClientProvider, IMongoDatabaseProvider
    {
        private readonly Lazy<MongoClient> _clientFactory;
        private readonly string _database;
        
        public MongoInternalsProvider(string connection, string database)
        {
            _clientFactory = new Lazy<MongoClient>(() => new MongoClient(connection));
            _database = database;
        }

        /// <inheritdoc />
        public MongoClient GetClient()
        {
            return _clientFactory.Value;
        }

        /// <inheritdoc />
        public IMongoDatabase GetDatabase()
        {
            return GetClient().GetDatabase(_database);
        }
    }
}