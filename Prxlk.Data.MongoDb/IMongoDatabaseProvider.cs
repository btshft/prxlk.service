using MongoDB.Driver;

namespace Prxlk.Data.MongoDb
{
    public interface IMongoDatabaseProvider
    {
        IMongoDatabase GetDatabase();
    }
}