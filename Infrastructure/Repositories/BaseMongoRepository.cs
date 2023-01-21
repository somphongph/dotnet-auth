using Domain.Entities;
using Domain.Extensions;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public abstract class BaseMongoRepository<TDocument> : IBaseMongoRepository<TDocument>
        where TDocument : BaseMongoEntity
    {
        protected readonly IMongoContext _mongoContext;
        private readonly IMongoCollection<TDocument> _collection;
        private readonly IHttpContextAccessor _contextAccessor;

        public BaseMongoRepository(IMongoContext mongoContext, IHttpContextAccessor contextAccessor)
        {
            _mongoContext = mongoContext;
            _collection = _mongoContext.GetCollection<TDocument>(typeof(TDocument).Name);
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public virtual async Task<TDocument> GetByIdAsync(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            return await _collection.FindAsync(filter).Result.FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<TDocument>> GetAsync()
        {
            var filter = Builders<TDocument>.Filter.Empty;
            var all = await _collection.FindAsync(filter);
            return await all.ToListAsync();
        }

        public virtual async Task AddAsync(TDocument obj)
        {
            var dtUtcNow = DateTime.UtcNow;
            var userId = _contextAccessor.UserId();

            obj.Status = RecordStatus.Active.Code();
            obj.CreatedOn = dtUtcNow;
            obj.CreatedBy = userId;
            obj.UpdatedOn = dtUtcNow;
            obj.UpdatedBy = userId;

            await _collection.InsertOneAsync(obj);
        }

        public virtual async Task UpdateAsync(TDocument obj)
        {
            var dtUtcNow = DateTime.UtcNow;
            var userId = _contextAccessor.UserId();

            obj.UpdatedOn = dtUtcNow;
            obj.UpdatedBy = userId;

            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, obj.Id);
            await _collection.ReplaceOneAsync(filter, obj);
        }

        public virtual async Task DeleteAsync(TDocument obj)
        {
            var dtUtcNow = DateTime.UtcNow;
            var userId = _contextAccessor.UserId();

            obj.Status = RecordStatus.Deleted.Code();
            obj.DeletedOn = dtUtcNow;
            obj.DeletedBy = userId;

            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, obj.Id);
            await _collection.ReplaceOneAsync(filter, obj);
        }

        public virtual async Task ForceDeleteAsync(TDocument obj)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, obj.Id);
            await _collection.DeleteOneAsync(filter);
        }
    }
}