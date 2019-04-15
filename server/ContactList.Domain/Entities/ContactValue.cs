using System;
using ContactList.Domain.Entities.Base;

namespace ContactList.Domain.Entities
{
    public class ContactValue : Entity
    {
        public string Value { get; set; }
        
        public Guid PersonId { get; set; }

        public Person Person { get; set; }

        public bool IsPhone { get; set; }

        public bool IsWhatsApp { get; set; }

        public bool IsEmail { get; set; }
    }
}


