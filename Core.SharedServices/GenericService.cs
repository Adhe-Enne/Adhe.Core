using Core.Contracts;
using Core.Contracts.Model;
using System.Linq.Expressions;

namespace Core.SharedServices
{
    public class GenericService<T> : SharedServices.IGenericService<T> where T : BaseEntity
    {
        protected readonly IRepositoryAsync<T> _repository;

        public GenericService(IRepositoryAsync<T> repository)
        {
            _repository = repository;
        }

        #region Query Methods
        public virtual IQueryable<T> AsQueryable() => _repository.AsQueryable();

        public virtual async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
            => await _repository.GetAllWithIncludesAsync(includeProperties);

        public virtual async Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties)
            => await _repository.FilterAsync(where, includeProperties);

        public virtual async Task<T?> FindAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties)
            => await _repository.FindAsync(where, includeProperties);

        public virtual async Task<T?> GetByIdAsync(Guid id)
            => await _repository.GetByIdAsync(id);
        #endregion

        #region Insert

        public virtual async Task InsertAsync(T entity, bool saveChanges = true)
            => await _repository.InsertAsync(entity, saveChanges);

        public virtual async Task InsertRangeAsync(IEnumerable<T> entities, bool saveChanges = true)
            => await _repository.InsertRangeAsync(entities, saveChanges);
        #endregion

        #region Update
        public virtual async Task UpdateAsync(T entity, bool saveChanges = true)
            => await _repository.UpdateAsync(entity, saveChanges);

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities, bool saveChanges = true)
            => await _repository.UpdateRangeAsync(entities, saveChanges);

        public virtual void PartialUpdate(T entity, params Expression<Func<T, object>>[] properties)
            => _repository.PartialUpdate(entity, properties);

        public virtual void PartialUpdateWithoutSave(T entity, params Expression<Func<T, object>>[] properties)
            => _repository.PartialUpdateWithoutSave(entity, properties);

        public virtual async Task PartialUpdateAsync(T entity, params Expression<Func<T, object>>[] properties)
            => await _repository.PartialUpdateAsync(entity, properties);

        public virtual async Task ActivateAsync(Guid id)
            => await _repository.ActivateAsync(id);

        public virtual async Task ActivateAsync(T entity)
            => await _repository.ActivateAsync(entity);
        #endregion

        #region Delete / Inactivate
        public virtual async Task InactivateAsync(Guid id)
            => await _repository.InactivateAsync(id);

        public virtual async Task InactivateAsync(T entity)
            => await _repository.InactivateAsync(entity);

        public virtual async Task DeleteAsync(Guid id, bool saveChanges = true)
            => await _repository.DeleteAsync(id, saveChanges);

        public virtual async Task DeleteAsync(T entity, bool saveChanges = true)
            => await _repository.DeleteAsync(entity, saveChanges);

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges = true)
            => await _repository.DeleteRangeAsync(entities, saveChanges);
        #endregion

        #region Utility
        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
            => await _repository.ExistsAsync(predicate);

        public virtual async Task<int> SaveChangesAsync()
            => await _repository.SaveChangesAsync();

        public async Task<TField>? SelectAsync<TField>(Expression<Func<T, bool>> where, Expression<Func<T, TField>> fieldSelector, params Expression<Func<T, object>>[] includeProperties)
        {
            var ret = await FilterAsync(where, includeProperties);
            return ret.AsQueryable()
            .Select(fieldSelector)
            .FirstOrDefault()!;
        }

        /*
        public Task<IEnumerable<TField>> SelectMany<TField>(Expression<Func<T, bool>> where, Expression<Func<T, TField>> fieldSelector, params Expression<Func<T, object>>[] includeProperties)
        {
            return  _repository.AsQueryable().Where(where)
            .Select(fieldSelector)
            .ToList();
        }
        */
        public virtual async Task<IEnumerable<TField>> SelectMany<TField>(Expression<Func<T, bool>> where, Expression<Func<T, TField>> fieldSelector, params Expression<Func<T, object>>[] includeProperties)
        {
            var filtered = await _repository.FilterAsync(where, includeProperties);
            return filtered.AsQueryable().Select(fieldSelector).ToList();
        }
        #endregion
    }
}
