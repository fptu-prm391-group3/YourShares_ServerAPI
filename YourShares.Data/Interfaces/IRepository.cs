using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace YourShares.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Inserted entity</returns>
        EntityEntry<T> Insert(T entity);

        /// <summary>
        /// Insert many entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void InsertMany(IEnumerable<T> entities);

        /// <summary>
        /// Gets by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        T GetById(Guid id);

        /// <summary>
        /// Gets the by many identifier.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        T GetByManyId(IEnumerable<Guid> ids);
        
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();
        
        /// <summary>
        /// Gets all as no tracking.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAllAsNoTracking();

        /// <summary>
        /// Gets many.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IQueryable<T> GetMany(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets many as no tracking.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IQueryable<T> GetManyAsNoTracking(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Updates specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        EntityEntry<T> Update(T entity);

        /// <summary>
        /// Updates the range.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void UpdateRange(IEnumerable<T> entities);
        
                
        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);
        
        /// <summary>
        /// Deletes a range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void DeleteRange(IEnumerable<T> entities);
    }
}