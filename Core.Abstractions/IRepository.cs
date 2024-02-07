using System.Linq.Expressions;

namespace Core.Abstractions
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> AsQueryable();
        IEnumerable<T> GetAll();
        T? Find(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> Filter(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);

        void Delete(T entity);
        void Insert(T entity);
        void Update(T entity);
        void Delete(IEnumerable<T> entities);
        void Insert(IEnumerable<T> entities);
        void Update(IEnumerable<T> entities);
        void PartialUpdate(T t, params Expression<Func<T, object>>[] properties);
        IEnumerable<T> SqlQuery(string query, params dynamic[] parameters);
        int ExecuteSqlCommand(string query, params dynamic[] parameters);
    }
}

