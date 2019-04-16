using ContactList.Domain.Entities;
using ContactList.Domain.Service.Interfaces.Repositories;
using ContactList.ORM.Contexts;
using ContactList.ORM.Repositories.Base;
using System;
using System.Linq;
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

        public void UpdateContact(ContactValue contact)
        {
            GetAll().Where(x => x.Id == contact.Id).Update(x => new ContactValue
            {
                IsEmail = contact.IsEmail,
                IsPhone = contact.IsPhone,
                IsWhatsApp = contact.IsWhatsApp,
                Value = contact.Value
            });
        }
    }
}