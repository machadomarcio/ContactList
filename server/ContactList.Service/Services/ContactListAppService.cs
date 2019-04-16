using ContactList.Common.DependencyResolution;
using ContactList.Domain.Entities;
using ContactList.Domain.Service.Interfaces.AppServices;
using ContactList.Domain.Service.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactList.Service.Services
{
    internal class ContactListAppService : IContactListAppServiceAppService
    {
        #region [Properties]


        internal Lazy<IContactRepository> _repository { get; set; }

        internal Lazy<IContactValueRepository> _repositoryContactValue { get; set; }


        #endregion

        #region [Constructor]

        public ContactListAppService()
        {
            _repository = IoC.GetLazy<IContactRepository>();
            _repositoryContactValue = IoC.GetLazy<IContactValueRepository>();

        }
        #endregion

        #region [Methods]

        public List<Person> GetAll()
        {
            return _repository.Value.GetAll().ToList();
        }

        public Person GetById(Guid id)
        {
            return _repository.Value.GetById(id);
        }

        public void Save(Person person)
        {
            if (person.Id == Guid.Empty || person.Id == null)
                _repository.Value.AddAndSaveChanges(person);
            else
                _repository.Value.UpdateExtension(person);
        }

        public void Delete(Guid id)
        {
            _repositoryContactValue.Value.DeleteAllByPersonId(id);
            _repository.Value.DeletePerson(id);
        }

        public void DeleteContact(Guid id)
        {
            _repositoryContactValue.Value.DeleteById(id);
        }

        public void SaveContact(ContactValue contact)
        {
            if (contact.Id == Guid.Empty || contact.Id == null)
                _repositoryContactValue.Value.AddAndSaveChanges(contact);
            else
                _repositoryContactValue.Value.UpdateContact(contact);
        }

        public List<ContactValue> GetContactByPersonId(Guid personId)
        {
            return _repositoryContactValue.Value.GetAll().Where(x => x.PersonId == personId).ToList();
        }
        #endregion
    }
};