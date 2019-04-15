using ContactList.Domain.Entities;
using ContactList.Domain.Service.Interfaces.ORM;
using System.Data.Entity.ModelConfiguration;

namespace ContactList.ORM.Configurations
{
    public class ContactValueConfiguration : EntityTypeConfiguration<ContactValue>, IContext
    {
        public ContactValueConfiguration()
        {
            ToTable("ContactValue");

            HasRequired(x => x.Person)
                .WithMany(x => x.ContactValues)
                .HasForeignKey(x => x.PersonId);
        }
    }
}
