using System;
using ContactList.Domain.Entities;
using ContactList.Domain.Service.Interfaces.Repositories.Base;


namespace ContactList.Domain.Service.Interfaces.Repositories
{
    public interface IContactValueRepository : IRepositoryEntity<ContactValue>
    {
        void DeleteAllByPersonId(Guid id);
        void DeleteById(Guid id);
    }
}