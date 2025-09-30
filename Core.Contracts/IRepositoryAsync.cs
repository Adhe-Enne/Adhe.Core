using Core.Contracts.Model.Interfaces;
using System.Linq.Expressions;

namespace Core.Contracts
{
    public interface IRepositoryAsync<T> where T : class, IEntity
    {
        IQueryable<T> AsQueryable();

        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);

        Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);

        Task<T?> FindAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);

        Task<T?> GetByIdAsync(Guid id);

        Task InsertAsync(T entity, bool saveChanges = true);

        Task InsertRangeAsync(IEnumerable<T> entities, bool saveChanges = true);

        Task UpdateAsync(T entity, bool saveChanges = true);

        Task UpdateRangeAsync(IEnumerable<T> entities, bool saveChanges = true);

        void PartialUpdate(T entity, params Expression<Func<T, object>>[] properties);

        void PartialUpdateWithoutSave(T entity, params Expression<Func<T, object>>[] properties);

        Task PartialUpdateAsync(T entity, params Expression<Func<T, object>>[] properties);

        Task ActivateAsync(Guid id);

        Task ActivateAsync(T entity);

        Task InactivateAsync(Guid id);

        Task InactivateAsync(T entity);

        Task DeleteAsync(Guid id, bool saveChanges = true);

        Task DeleteAsync(T entity, bool saveChanges = true);

        Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges = true);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        Task<int> SaveChangesAsync();
    }
}
