using ContactList.Domain.Entities;
using ContactList.Domain.Service.Interfaces.ORM;
using System.Data.Entity.ModelConfiguration;

namespace ContactList.ORM.Configurations
{
    public class PersonConfiguration : EntityTypeConfiguration<Person>, IContext
    {
        public PersonConfiguration()
        {
            ToTable("Person");

            Property(x => x.FirstName).IsRequired();
            Property(x => x.LastName).IsRequired();

            HasMany(x => x.ContactValues)
               .WithRequired(x => x.Person)
               .HasForeignKey(x => x.PersonId);

        }
    }
}
