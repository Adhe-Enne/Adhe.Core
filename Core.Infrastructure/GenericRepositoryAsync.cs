using Core.Contracts;
using Core.Contracts.Model;
using Core.Contracts.Model.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Infrastructure
{
    public class GenericRepositoryContextAsync<T, TContext> : IRepositoryAsync<T>, IDisposable
        where T : BaseEntity
        where TContext : DbContext
    {
        protected readonly TContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepositoryContextAsync(TContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<T>();
        }

        #region Helpers

        private static IQueryable<T> PerformInclusions(IEnumerable<Expression<Func<T, object>>> includeProperties, IQueryable<T> query)
        {
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        private static void ApplyAuditInfo(T entity, bool isNew)
        {
            var timestamp = DateTime.UtcNow;

            if (entity is IAuditableCreateUpdate auditableCU)
            {
                if (isNew)
                    auditableCU.DateAdded = timestamp;

                auditableCU.DateUpdated = timestamp;
            }
            else if (isNew && entity is IAuditableCreate auditableC)
            {
                auditableC.DateAdded = timestamp;
            }
        }

        #endregion

        #region Query Methods

        public IQueryable<T> AsQueryable() => _dbSet.AsQueryable();

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return await query.AsNoTracking().Where(where).ToListAsync();
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return await query.AsNoTracking().FirstOrDefaultAsync(where);
        }

        public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

        #endregion

        #region Insert

        public async Task InsertAsync(T entity, bool saveChanges = true)
        {
            ApplyAuditInfo(entity, true);
            await _dbSet.AddAsync(entity);

            if (saveChanges)
                await _dbContext.SaveChangesAsync();
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities, bool saveChanges = true)
        {
            foreach (var e in entities)
                ApplyAuditInfo(e, true);

            await _dbSet.AddRangeAsync(entities);

            if (saveChanges)
                await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Update

        public async Task UpdateAsync(T entity, bool saveChanges = true)
        {
            ApplyAuditInfo(entity, false);
            _dbSet.Update(entity);

            if (saveChanges)
                await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities, bool saveChanges = true)
        {
            foreach (var e in entities)
                ApplyAuditInfo(e, false);

            _dbSet.UpdateRange(entities);

            if (saveChanges)
                await _dbContext.SaveChangesAsync();
        }

        public void PartialUpdate(T entity, params Expression<Func<T, object>>[] properties)
        {
            var entry = _dbContext.Entry(entity);

            foreach (var p in properties)
                entry.Property(p).IsModified = true;
        }

        public void PartialUpdateWithoutSave(T entity, params Expression<Func<T, object>>[] properties)
        {
            var entry = _dbContext.Entry(entity);

            foreach (var p in properties)
                entry.Property(p).IsModified = true;
        }

        public async Task PartialUpdateAsync(T entity, params Expression<Func<T, object>>[] properties)
        {
            var entry = _dbContext.Entry(entity);

            foreach (var p in properties)
                entry.Property(p).IsModified = true;

            await _dbContext.SaveChangesAsync();
        }

        public async Task ActivateAsync(Guid id)
        {
            T? entity = await GetByIdAsync(id);
            
            if (entity is not null)
                await ActivateAsync(entity);
        }

        public async Task ActivateAsync(T entity)
        {
            if (entity != null && entity is IEntity activable)
            {
                activable.IsActive = true;
                await PartialUpdateAsync(entity, u => u.IsActive);
            }
        }
        #endregion

        #region Delete
        public async Task InactivateAsync(Guid id)
        {
            T? entity = await GetByIdAsync(id);

            if (entity is not null)
                await InactivateAsync(entity);
        }

        public async Task InactivateAsync(T entity)
        {
            if (entity != null && entity is IEntity deletable)
            {
                deletable.IsActive = false;
                await PartialUpdateAsync(entity, u => u.IsActive);
            }
        }

        public async Task DeleteAsync(Guid id, bool saveChanges = true)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        public async Task DeleteAsync(T entity, bool saveChanges = true)
        {
            _dbSet.Remove(entity);

            if (saveChanges)
                await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges = true)
        {
            _dbSet.RemoveRange(entities);

            if (saveChanges)
                await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Utility

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        #endregion
    }

    public class GenericRepositoryAsync<T> : IRepositoryAsync<T> where T : BaseEntity
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepositoryAsync(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<T>();
        }

        #region Helpers

        private static IQueryable<T> PerformInclusions(IEnumerable<Expression<Func<T, object>>> includeProperties, IQueryable<T> query)
        {
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        private static void ApplyAuditInfo(T entity, bool isNew)
        {
            var timestamp = DateTime.UtcNow;

            if (entity is IAuditableCreateUpdate auditableCU)
            {
                if (isNew)
                    auditableCU.DateAdded = timestamp;

                auditableCU.DateUpdated = timestamp;
            }
            else if (isNew && entity is IAuditableCreate auditableC)
            {
                auditableC.DateAdded = timestamp;
            }
        }

        #endregion

        #region Query Methods

        public IQueryable<T> AsQueryable() => _dbSet.AsQueryable();

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return await query.AsNoTracking().Where(where).ToListAsync();
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return await query.AsNoTracking().FirstOrDefaultAsync(where);
        }

        public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

        #endregion

        #region Insert

        public async Task InsertAsync(T entity, bool saveChanges = true)
        {
            ApplyAuditInfo(entity, true);
            await _dbSet.AddAsync(entity);

            if (saveChanges)
                await _dbContext.SaveChangesAsync();
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities, bool saveChanges = true)
        {
            foreach (var e in entities)
                ApplyAuditInfo(e, true);

            await _dbSet.AddRangeAsync(entities);

            if (saveChanges)
                await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Update

        public async Task UpdateAsync(T entity, bool saveChanges = true)
        {
            ApplyAuditInfo(entity, false);
            _dbSet.Update(entity);

            if (saveChanges)
                await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities, bool saveChanges = true)
        {
            foreach (var e in entities)
                ApplyAuditInfo(e, false);

            _dbSet.UpdateRange(entities);

            if (saveChanges)
                await _dbContext.SaveChangesAsync();
        }

        public void PartialUpdate(T entity, params Expression<Func<T, object>>[] properties)
        {
            var entry = _dbContext.Entry(entity);

            foreach (var p in properties)
                entry.Property(p).IsModified = true;
        }

        public void PartialUpdateWithoutSave(T entity, params Expression<Func<T, object>>[] properties)
        {
            var entry = _dbContext.Entry(entity);

            foreach (var p in properties)
                entry.Property(p).IsModified = true;
        }

        public async Task PartialUpdateAsync(T entity, params Expression<Func<T, object>>[] properties)
        {
            var entry = _dbContext.Entry(entity);

            foreach (var p in properties)
                entry.Property(p).IsModified = true;

            await _dbContext.SaveChangesAsync();
        }

        public async Task ActivateAsync(Guid id)
        {
            T? entity = await GetByIdAsync(id);

            if (entity is not null)
                await ActivateAsync(entity);
        }

        public async Task ActivateAsync(T entity)
        {
            if (entity != null && entity is IEntity activable)
            {
                activable.IsActive = true;
                await PartialUpdateAsync(entity, u => u.IsActive);
            }
        }
        #endregion

        #region Delete
        public async Task InactivateAsync(Guid id)
        {
            T? entity = await GetByIdAsync(id);

            if (entity is not null)
                await InactivateAsync(entity);
        }

        public async Task InactivateAsync(T entity)
        {
            if (entity != null && entity is IEntity deletable)
            {
                deletable.IsActive = false;
                await PartialUpdateAsync(entity, u => u.IsActive);
            }
        }

        public async Task DeleteAsync(Guid id, bool saveChanges = true)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        public async Task DeleteAsync(T entity, bool saveChanges = true)
        {
            _dbSet.Remove(entity);

            if (saveChanges)
                await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges = true)
        {
            _dbSet.RemoveRange(entities);

            if (saveChanges)
                await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Utility

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        #endregion
    }

}