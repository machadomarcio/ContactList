using ContactList.Common.Contexts;
using ContactList.Common.DependencyResolution;
using ContactList.Domain.Entities.Base;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace ContactList.ORM.Repositories.Base
{
    public class RepositoryBaseEntity<T, Context> : Disposable
        where T : Entity
        where Context : BaseContext 
    {
        protected IList<DbContextTransaction> transactions = new List<DbContextTransaction>();

        protected readonly IDbSet<T> dbset;

        protected IDatabaseFactory DatabaseFactory { get; private set; }

        protected Context DataContext;

        public RepositoryBaseEntity(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;

            DataContext = DatabaseFactory.GetContext<Context>();

            dbset = DataContext.Set<T>();
        }

        protected RepositoryBaseEntity() : this(IoC.Get<IDatabaseFactory>())
        {
        }

        protected RepositoryBaseEntity(Context context)
        {
            DataContext = context;

            dbset = DataContext.Set<T>();
        }

        public virtual DbContextTransaction BeginTransaction()
        {
            transactions.Add(DataContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable));

            return transactions.Last();
        }

        public virtual DbContextTransaction BeginTransaction(System.Data.IsolationLevel level)
        {
            transactions.Add(DataContext.Database.BeginTransaction(level));

            return transactions.Last();
        }

        public virtual T UnProxy(T proxyObject)
        {
            var proxyCreationEnabled = DataContext.Configuration.ProxyCreationEnabled;
            try
            {
                DataContext.Configuration.ProxyCreationEnabled = false;
                T poco = DataContext.Entry(proxyObject).CurrentValues.ToObject() as T;
                return poco;
            }
            finally
            {
                DataContext.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
            }
        }

        public virtual void Commit()
        {
            if (transactions.Count == 0x0) return;

            transactions.First().Commit();

            transactions.RemoveAt(0x0);
        }

        public virtual void Rollback()
        {
            if (transactions.Count == 0x0) return;

            transactions.First().Rollback();

            transactions.RemoveAt(0x0);
        }

        public virtual int SaveChanges()
        {
            return DataContext.SaveChanges();
        }

        public virtual T Add(T entity)
        {
            dbset.Add(entity);

            return entity;
        }

        public virtual T AddAndSaveChanges(T entity)
        {
            Add(entity);

            SaveChanges();

            return entity;
        }

        public virtual void Update(T entity)
        {
            DbEntityEntry dbEntityEntry = DataContext.Entry(entity);

            if (dbEntityEntry.State == EntityState.Detached)
            {
                dbset.Attach(entity);

                dbEntityEntry.State = EntityState.Modified;
            }
            else
            {
                foreach (var property in dbEntityEntry.OriginalValues.PropertyNames)
                {
                    var original = dbEntityEntry.OriginalValues.GetValue<object>(property);
                    var current = dbEntityEntry.CurrentValues.GetValue<object>(property);
                    if (original != null && !original.Equals(current))
                        dbEntityEntry.Property(property).IsModified = true;
                }
            }
        }

        public virtual int UpdateAndSaveChanges(T entity, Expression<Func<T, object>> property)
        {
            var dbEntityEntry = DataContext.Entry(entity);

            if (dbEntityEntry.State == EntityState.Detached)
                dbset.Attach(entity);

            dbEntityEntry.Property(property).IsModified = true;

            return SaveChanges();
        }

        public virtual int UpdateAndSaveChanges(T entity)
        {
            Update(entity);

            return SaveChanges();
        }

        public virtual T AddOrUpdateAndSaveChanges(T entity)
        {
            var dbEntityCount = Count(x => x.Id == entity.Id);

            if (dbEntityCount == 0)
                return AddAndSaveChanges(entity);

            UpdateAndSaveChanges(entity);
            return entity;
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = DataContext.Entry(entity);

            if (dbEntityEntry.State == EntityState.Detached)
                dbset.Attach(entity);

            dbEntityEntry.State = EntityState.Deleted;

            dbset.Remove(entity);
        }

        public virtual int DeleteAndSaveChanges(T entity)
        {
            Delete(entity);

            return SaveChanges();
        }

        public virtual void Delete(Guid id)
        {
            var entity = dbset.Find(id);

            if (entity != null)
                Delete(entity);
        }

        public virtual int DeleteAndSaveChanges(Guid id)
        {
            Delete(id);

            return SaveChanges();
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var objects = dbset.Where(where).AsEnumerable();

            foreach (var obj in objects)
                dbset.Remove(obj);
        }

        public virtual T GetById(Guid id)
        {
            return dbset.Find(id);
        }

        public virtual T GetByIdIncluding(Guid id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = dbset;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.FirstOrDefault(t => t.Id == id);
        }

        public virtual int Count(Expression<Func<T, bool>> where)
        {
            return dbset.Count(where);
        }

        public virtual IQueryable<T> GetAll()
        {
            return dbset;
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).ToList();
        }

        public virtual T GetSingle(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).FirstOrDefault();
        }

        public virtual IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = dbset;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        protected IQueryable<T> GetQueryable()
        {
            return dbset.AsQueryable();
        }

        public virtual void DeleteAll()
        {
            var ids = GetAll().Select(x => x.Id).ToArray();

            foreach (var id in ids)
            {
                Delete(id);
            }
        }

        public virtual int DeleteAllAndSaveChanges()
        {
            DeleteAll();

            return SaveChanges();
        }
    }
}
