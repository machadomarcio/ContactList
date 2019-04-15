using ContactList.Domain.Entities.Base;
using System.Collections.Generic;

namespace ContactList.Domain.Entities
{
    public class Person : Entity
    {
        public Person()
        {
            ContactValues = new List<ContactValue>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<ContactValue> ContactValues { get; set; }
    }
}
