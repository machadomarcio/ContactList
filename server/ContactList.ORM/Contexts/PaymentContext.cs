using System;
using System.Data.Common;
using System.Data.Entity;
using ContactList.Common.Contexts;
using ContactList.Domain.Entities;
using ContactList.Domain.Base;
using ContactList.Domain.Service.Interfaces.ORM;

namespace ContactList.ORM.Contexts
{
    public class Context : BaseContext
    {
        public DbSet<Person> Contacts { get; set; }

        public DbSet<ContactValue> ContactValues { get; set; }

        
        private const string InsertDateConst = "InsertDate";

        private const string ConnectionString = "Server=tcp:mmazuredatabases.database.windows.net,1433;Initial Catalog=ContactList;Persist Security Info=False;User ID=mmachado;Password=P@ssword!@#$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public Context(string connectionString = null) : base(connectionString ?? ConnectionString)
        {
            Database.SetInitializer<Context>(null);

            Database.Log = s => System.Diagnostics.Debug.WriteLine(s, "EntityFramework - Context");
        }

        public Context(DbConnection connection)
            : base(connection)
        {
            Database.CreateIfNotExists();
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            OnModelCreating<IContext>(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State.Equals(EntityState.Added) && entry.Property(IdConst).CurrentValue.Equals(Guid.Empty))
                {
                    entry.Property(IdConst).CurrentValue = GuidGenerator.NewId();
                }

                if (entry.Entity.GetType().GetProperty(InsertDateConst) == null) continue;

                if (entry.State.Equals(EntityState.Added))
                    entry.Property(InsertDateConst).CurrentValue = DateTime.Now;

                if (entry.State.Equals(EntityState.Modified))
                    entry.Property(InsertDateConst).IsModified = false;
            }

            return base.SaveChanges();
        }
    }
}