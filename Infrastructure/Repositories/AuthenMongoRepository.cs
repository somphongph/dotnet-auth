using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class AuthenMongoRepository : BaseMongoRepository<AuthenMongo>, IAuthenMongoRepository
{
    protected IMongoCollection<AuthenMongo> _collection;
    public AuthenMongoRepository(IMongoContext mongoContext, IHttpContextAccessor contextAccessor) : base(mongoContext, contextAccessor)
    {
        _collection = mongoContext.GetCollection<AuthenMongo>(typeof(AuthenMongo).Name);

        var indexOptions = new CreateIndexOptions { };
        var indexKeys = Builders<AuthenMongo>.IndexKeys.Ascending(x => x.RefreshToken);
        var indexModel = new CreateIndexModel<AuthenMongo>(indexKeys, indexOptions);
        _collection.Indexes.CreateOneAsync(indexModel);

        // Set Expire (TTL: Time-To-Live)
        // Delete record when expire
        indexOptions = new CreateIndexOptions { ExpireAfter = new TimeSpan(0, 10, 0) };
        indexKeys = Builders<AuthenMongo>.IndexKeys.Ascending(x => x.ExpiredOn);
        indexModel = new CreateIndexModel<AuthenMongo>(indexKeys, indexOptions);
        _collection.Indexes.CreateOneAsync(indexModel);
    }

    public virtual async Task<AuthenMongo> GetByRefreshTokenAsync(string refreshToken)
    {
        var filter = Builders<AuthenMongo>.Filter.Eq(doc => doc.RefreshToken, refreshToken);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}
