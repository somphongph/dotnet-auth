using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IBaseMongoRepository<TDocument>
        where TDocument : BaseMongoEntity
    {
        Task<TDocument> GetByIdAsync(string id);
        Task<IEnumerable<TDocument>> GetAsync();
        Task AddAsync(TDocument obj);
        Task UpdateAsync(TDocument obj);
        Task DeleteAsync(TDocument obj);
        Task ForceDeleteAsync(TDocument obj);
    }
}