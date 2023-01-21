using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IAccountRepository : IBaseSqlRepository<Account>
{
    Task<Account?> GetByUsernameAsync(string username);
}
