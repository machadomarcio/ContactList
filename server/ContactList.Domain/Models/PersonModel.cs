using System;
using System.Collections.Generic;

namespace ContactList.Domain.Models
{
    public  class PersonModel
    {
        public PersonModel()
        {
            ContactValues = new List<ContactValueModel>();
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<ContactValueModel> ContactValues { get; set; }
    }
}
