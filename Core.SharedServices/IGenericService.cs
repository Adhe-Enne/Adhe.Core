using Core.Contracts.Model;
using System.Linq.Expressions;

namespace Core.SharedServices
{
    public interface IGenericService<T> where T : BaseEntity
    {
        #region Métodos de consulta
        IQueryable<T> AsQueryable();
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);
        Task<T?> FindAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);
        Task<T?> GetByIdAsync(Guid id);
        #endregion

        #region Métodos de inserción
        Task InsertAsync(T entity, bool saveChanges = true);
        Task InsertRangeAsync(IEnumerable<T> entities, bool saveChanges = true);
        #endregion

        #region Métodos de actualización
        Task UpdateAsync(T entity, bool saveChanges = true);
        Task UpdateRangeAsync(IEnumerable<T> entities, bool saveChanges = true);
        void PartialUpdate(T entity, params Expression<Func<T, object>>[] properties);
        void PartialUpdateWithoutSave(T entity, params Expression<Func<T, object>>[] properties);
        Task PartialUpdateAsync(T entity, params Expression<Func<T, object>>[] properties);
        Task ActivateAsync(Guid id);
        Task ActivateAsync(T entity);
        #endregion

        #region Métodos de eliminación/inactivación
        Task InactivateAsync(Guid id);
        Task InactivateAsync(T entity);
        Task DeleteAsync(Guid id, bool saveChanges = true);
        Task DeleteAsync(T entity, bool saveChanges = true);
        Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges = true);
        #endregion

        #region Utilidades
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<int> SaveChangesAsync();
        Task<TField>? SelectAsync<TField>(Expression<Func<T, bool>> where, Expression<Func<T, TField>> fieldSelector, params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<TField>> SelectMany<TField>(Expression<Func<T, bool>> where, Expression<Func<T, TField>> fieldSelector, params Expression<Func<T, object>>[] includeProperties);
        #endregion
    }
}
