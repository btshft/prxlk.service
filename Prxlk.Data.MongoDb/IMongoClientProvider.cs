using MongoDB.Driver;

namespace Prxlk.Data.MongoDb
{
    public interface IMongoClientProvider
    {
        MongoClient GetClient();
    }
}