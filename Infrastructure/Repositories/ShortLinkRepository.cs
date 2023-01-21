using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class ShortLinkRepository : BaseSqlRepository<ShortLink>, IShortLinkRepository
    {
        public ShortLinkRepository(Context dbcontext) : base(dbcontext)
        {
        }
    }
}