using ContactList.Common.Contexts;
using ContactList.Common.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace ContactList.ORM.Repositories.Base
{
    public class RepositoryBase<T, Context> : Disposable
        where T : class
        where Context : BaseContext
    {
        protected IList<DbContextTransaction> transactions = new List<DbContextTransaction>();

        protected readonly IDbSet<T> dbset;

        protected IDatabaseFactory DatabaseFactory { get; private set; }

        protected DbContext DataContext;

        public RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;

            DataContext = DatabaseFactory.GetContext<Context>();

            dbset = DataContext.Set<T>();
        }

        protected RepositoryBase() : this(IoC.Get<IDatabaseFactory>()) { }

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

        public virtual T GetById(int id)
        {
            return dbset.Find(id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return dbset;
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

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return GetAll().Where(where).ToList();
        }

        public virtual T GetSingle(Expression<Func<T, bool>> where)
        {
            return GetAll().FirstOrDefault(where);
        }

        public virtual IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = GetAll();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
        }
    }
}
