using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class ShortLinkMongoRepository : BaseMongoRepository<ShortLinkMongo>, IShortLinkMongoRepository
    {
        protected IMongoCollection<ShortLinkMongo> _collection;
        public ShortLinkMongoRepository(IMongoContext mongoContext, IHttpContextAccessor contextAccessor) : base(mongoContext, contextAccessor)
        {
            _collection = mongoContext.GetCollection<ShortLinkMongo>(typeof(ShortLinkMongo).Name);

            var indexOptions = new CreateIndexOptions();
            var indexKeys = Builders<ShortLinkMongo>.IndexKeys.Ascending(x => x.ShortKey);
            var indexModel = new CreateIndexModel<ShortLinkMongo>(indexKeys, indexOptions);
            _collection.Indexes.CreateOneAsync(indexModel);

            indexKeys = Builders<ShortLinkMongo>.IndexKeys.Ascending(x => x.ShortUrl);
            indexModel = new CreateIndexModel<ShortLinkMongo>(indexKeys, indexOptions);
            _collection.Indexes.CreateOneAsync(indexModel);

            // Set Expire (TTL: Time-To-Live)
            // Delete record when expire
            indexOptions = new CreateIndexOptions { ExpireAfter = new TimeSpan(0, 1, 0) };
            indexKeys = Builders<ShortLinkMongo>.IndexKeys.Ascending(x => x.ExpiredOn);
            indexModel = new CreateIndexModel<ShortLinkMongo>(indexKeys, indexOptions);
            _collection.Indexes.CreateOneAsync(indexModel);
        }

        public virtual async Task<ShortLinkMongo> GetByShortKeyAsync(string shortKey)
        {
            var filter = Builders<ShortLinkMongo>.Filter.Eq(doc => doc.ShortKey, shortKey);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public virtual async Task<ShortLinkMongo> GetByShortUrlAsync(string shortUrl)
        {
            var filter = Builders<ShortLinkMongo>.Filter.Eq(doc => doc.ShortUrl, shortUrl);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}