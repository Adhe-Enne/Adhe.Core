using Core.Contracts.Model;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure
{
    public class BaseContext<TContext> : DbContext where TContext : DbContext
    {
        protected BaseContext(DbContextOptions<TContext> options) : base(options)
        {

        }
        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity)
                .Where(e => e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                entityEntry.Property("DateAdded").CurrentValue = DateTime.UtcNow;
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity)
                .Where(e => e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                entityEntry.Property("DateUpdated").CurrentValue = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
