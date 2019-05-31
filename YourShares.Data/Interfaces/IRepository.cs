using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace YourShares.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Get();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        IQueryable<T> GetAll();
    }
}