using System;
using ContactList.Domain.Entities;
using ContactList.Domain.Service.Interfaces.Repositories.Base;


namespace ContactList.Domain.Service.Interfaces.Repositories
{
    public interface IContactRepository : IRepositoryEntity<Person>
    {
        void UpdateExtension(Person contact);
        void DeletePerson(Guid id);
    }
}