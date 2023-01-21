using MongoDB.Driver;

namespace Infrastructure.Persistence
{
    public interface IMongoContext
    {
        IMongoCollection<TDocument> GetCollection<TDocument>(string name);
    }
}