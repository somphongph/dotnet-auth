using Domain.Entities;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class Context : DbContext
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public Context(DbContextOptions<Context> options, IHttpContextAccessor contextAccessor) : base(options)
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public virtual DbSet<ShortLink> Shortener { get; set; } = null!;

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var dtUtcNow = DateTime.UtcNow;
            Guid? userId = new Guid(_contextAccessor.UserId() ?? "");
            var entries = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                switch (entry.State)
                {
                    case EntityState.Added:
                        {
                            entity.Status = RecordStatus.Active.Code();
                            entity.CreatedOn = dtUtcNow;
                            entity.CreatedBy = userId;
                            entity.UpdatedOn = dtUtcNow;
                            entity.UpdatedBy = userId;
                        }
                        break;
                    case EntityState.Modified:
                        {
                            if (entity.Status == RecordStatus.Deleted.Code())
                            {
                                entity.DeletedOn = dtUtcNow;
                                entity.DeletedBy = userId;
                            }
                            entity.UpdatedOn = dtUtcNow;
                            entity.UpdatedBy = userId;
                        }
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}