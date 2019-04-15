using System;
using System.Data;
using System.Data.Entity;

namespace ContactList.Domain.Service.Interfaces.Repositories.Base
{
    public interface IRepositoryBaseEntity
    {
        int SaveChanges();

        DbContextTransaction BeginTransaction();

        DbContextTransaction BeginTransaction(IsolationLevel level);

        void Commit();

        void Rollback();

        void Delete(Guid id);
    }
}