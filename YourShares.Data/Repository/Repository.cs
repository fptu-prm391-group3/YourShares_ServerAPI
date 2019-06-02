using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using YourShares.Data.Interfaces;

namespace YourShares.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public EntityEntry<T> Insert(T entity)
        {
            return _unitOfWork.Context.Set<T>().Add(entity);
        }
        
        public void InsertMany(IEnumerable<T> entities)
        {
            _unitOfWork.Context.Set<T>().AddRange(entities);
        }

        public void Delete(T entity)
        {
            T existing = _unitOfWork.Context.Set<T>().Find(entity);
            if (existing != null) _unitOfWork.Context.Set<T>().Remove(existing);
        }
        
        public void DeleteRange(IEnumerable<T> entities)
        {
            IEnumerable<T> existing = new List<T>();

            foreach (var item in entities)
            {
                T a = _unitOfWork.Context.Set<T>().Find(item);
                existing.ToList().Add(a);
            }
            // TODO Bad code here, existing always not null
            // Should change to existing.Any()
            if (existing != null) _unitOfWork.Context.Set<T>().RemoveRange(existing);
        }

        public T GetById(Guid id)
        {
            return _unitOfWork.Context.Set<T>().Find(id);
        }

        public T GetByManyId(IEnumerable<Guid> ids)
        {
            return _unitOfWork.Context.Set<T>().Find(ids);
        }


        public IQueryable<T> GetAll()
        {
            return _unitOfWork.Context.Set<T>().AsQueryable();
        }
        
        public IQueryable<T> GetAllAsNoTracking()
        {
            return _unitOfWork.Context.Set<T>().AsQueryable<T>().AsNoTracking();
        }
        
        public IQueryable<T> GetMany(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _unitOfWork.Context.Set<T>().Where(predicate).AsQueryable<T>();
        }
        
        public IQueryable<T> GetManyAsNoTracking(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _unitOfWork.Context.Set<T>().Where(predicate).AsQueryable<T>();
        }
        
        public EntityEntry<T> Update(T entity)
        {
            _unitOfWork.Context.Entry(entity).State = EntityState.Modified;
            return _unitOfWork.Context.Set<T>().Attach(entity);
        }
        
        public void UpdateRange(IEnumerable<T> entities)
        {
            foreach (var item in entities)
            {
                _unitOfWork.Context.Entry(item).State = EntityState.Modified;
            }
            _unitOfWork.Context.Set<T>().AttachRange(entities);
        }
    }
}