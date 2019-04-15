using System;
using System.Collections.Generic;
using ContactList.Domain.Entities;

namespace ContactList.Test.Builder.Domain
{
    public class ContactBuilder
    {
        private static Person _contact;

        public static ContactBuilder Default()
        {
            _contact = new Person()
            {
                Id = Guid.NewGuid(),
                FirstName = "Primeiro Nome",
                LastName = "Sobrenome",
                InsertDate = DateTime.Now,
                ContactValues = new List<ContactValue>()
            };

            return new ContactBuilder();
        }

        public ContactBuilder WithFirstName(string firstName)
        {
            _contact.FirstName = firstName;
            return this;
        }

        public Person Build()
        {
            return _contact;
        }
    }
}