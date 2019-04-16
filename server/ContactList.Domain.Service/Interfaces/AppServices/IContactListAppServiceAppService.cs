using System;
using System.Collections.Generic;
using ContactList.Domain.Entities;
using ContactList.Domain.Models;

namespace ContactList.Domain.Service.Interfaces.AppServices
{
    public interface IContactListAppServiceAppService
    {
        List<Person> GetAll();
        Person GetById(Guid id);
        void Save(Person contact);
        void Delete(Guid id);
        void DeleteContact(Guid id);
        void SaveContact(ContactValue contact);
        List<ContactValue> GetContactByPersonId(Guid personId);
    }
}