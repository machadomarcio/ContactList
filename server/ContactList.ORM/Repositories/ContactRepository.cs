
using ContactList.Domain.Entities;
using ContactList.Domain.Service.Interfaces.Repositories;
using ContactList.ORM.Contexts;
using ContactList.ORM.Repositories.Base;
using System;
using System.Linq;
using Z.EntityFramework.Plus;


namespace ContactList.ORM.Repositories
{
    public class ContactRepository : RepositoryBaseEntity<Person, Context>, IContactRepository
    {
        public ContactRepository() { }

        public ContactRepository(Context context) : base(context) { }

        public void DeletePerson(Guid id)
        {
            GetAll().Where(x => x.Id == id).Delete();
        }

        public void UpdateExtension(Person contact)
        {
            GetAll()
                .Where(x => x.Id == contact.Id)
                .Update(x => new Person
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName
                });
        }
    }
}