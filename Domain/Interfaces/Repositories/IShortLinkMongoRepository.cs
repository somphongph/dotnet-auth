using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IShortLinkMongoRepository : IBaseMongoRepository<ShortLinkMongo>
    {
        Task<ShortLinkMongo> GetByShortKeyAsync(string shortKey);

        Task<ShortLinkMongo> GetByShortUrlAsync(string shortUrl);
    }
}