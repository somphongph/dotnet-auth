using Domain.Entities;
using Domain.Extensions;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AccountRepository : BaseSqlRepository<Account>, IAccountRepository
{
    public AccountRepository(Context dbcontext) : base(dbcontext)
    {
    }

    public virtual async Task<Account?> GetByUsernameAsync(string username)
    {
        return await _dbContext.Set<Account>()
            .FirstOrDefaultAsync(x => x.Username == username && x.Status == RecordStatus.Active.Code());
    }
}
