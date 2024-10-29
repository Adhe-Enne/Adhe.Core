using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Contracts
{
    public interface IService<T>
    {
        IEnumerable<T> Filter(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);

        IEnumerable<T> GetAll();
        
        T Find(int id);
        
        T Find(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);

        IEnumerable<TField> SelectMany<TField>(Expression<Func<T, bool>> where, Expression<Func<T, TField>> fieldSelector);
        TField? SelectAsync<TField>(Expression<Func<T, bool>> where, Expression<Func<T, TField>> fieldSelector);
    }
}
