using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace YourShares.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        EntityEntry<T> Insert(T entity);

        void InsertMany(IEnumerable<T> entities);
        
        void Delete(T entity);
        
        void DeleteRange(IEnumerable<T> entities);

        T GetById(Guid id);

        T GetByManyId(IEnumerable<Guid> ids);
        
        IQueryable<T> GetAll();
        
        IQueryable<T> GetAllAsNoTracking();

        IQueryable<T> GetMany(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetManyAsNoTracking(Expression<Func<T, bool>> predicate);

        EntityEntry<T> Update(T entity);

        void UpdateRange(IEnumerable<T> entities);
    }
}