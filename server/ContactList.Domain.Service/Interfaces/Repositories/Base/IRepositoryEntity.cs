using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ContactList.Domain.Service.Interfaces.Repositories.Base
{
    public interface IRepositoryEntity<T> : IRepositoryBaseEntity where T : class
    {
        T Add(T entity);

        T AddAndSaveChanges(T entity);

        void Update(T entity);

        int UpdateAndSaveChanges(T entity);

        int UpdateAndSaveChanges(T entity, Expression<Func<T, object>> property);

        T AddOrUpdateAndSaveChanges(T entity);

        void Delete(T entity);

        int DeleteAndSaveChanges(T entity);

        int DeleteAndSaveChanges(Guid id);

        T GetById(Guid id);

        T GetByIdIncluding(Guid id, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> GetAll();

        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);

        T GetSingle(Expression<Func<T, bool>> where);

        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);

        void DeleteAll();

        int DeleteAllAndSaveChanges();
    }
}