using Domain.Extensions;
using MongoDB.Driver;

namespace Infrastructure.Persistence
{
    public class MongoContext : IMongoContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public MongoContext(IMongoSettings settings)
        {
            _mongoClient = new MongoClient(settings.ConnectionString);
            _db = _mongoClient.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<TDocument> GetCollection<TDocument>(string name)
        {
            return _db.GetCollection<TDocument>(name.ToSnakeCase());
        }
    }
}