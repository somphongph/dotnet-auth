using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IAuthenMongoRepository : IBaseMongoRepository<AuthenMongo>
{
    Task<AuthenMongo> GetByRefreshTokenAsync(string refreshToken);
}
