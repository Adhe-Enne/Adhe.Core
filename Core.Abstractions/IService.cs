using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Abstractions
{
    public interface IService<T>
    {
        IEnumerable<T> Filter(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);

        IEnumerable<T> GetAll();
        
        T Find(int id);
        
        T Find(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);
        
    }
}
