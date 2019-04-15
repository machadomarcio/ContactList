using System;
using System.Linq;
using ContactList.Domain.Entities;
using ContactList.Domain.Service.Interfaces.Repositories;
using ContactList.ORM.Contexts;
using ContactList.ORM.Repositories.Base;
using Z.EntityFramework.Plus;

namespace ContactList.ORM.Repositories
{
    public class ContactValueRepository : RepositoryBaseEntity<ContactValue, Context>, IContactValueRepository
    {
        public ContactValueRepository()
        {

        }

        public ContactValueRepository(Context context) : base(context)
        {

        }

        public void DeleteAllByPersonId(Guid personId)
        {
            GetAll().Where(x => x.PersonId == personId).Delete();
        }

        public void DeleteById(Guid id)
        {
            GetAll().Where(x => x.Id == id).Delete();
        }
    }
}