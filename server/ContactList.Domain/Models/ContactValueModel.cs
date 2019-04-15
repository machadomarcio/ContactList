using System;

namespace ContactList.Domain.Models
{
    public class ContactValueModel
    {
        public Guid  Id { get; set; }
        public string Value { get; set; }

        public Guid PersonId { get; set; }

        public bool IsPhone { get; set; }

        public bool IsWhatsApp { get; set; }

        public bool IsEmail { get; set; }
    }
}
