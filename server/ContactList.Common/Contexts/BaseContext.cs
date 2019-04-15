using System;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;

namespace ContactList.Common.Contexts
{
    public abstract class BaseContext : DbContext
    {
        protected const string IdConst = "Id";

        protected BaseContext(String connectionString) : base(connectionString)
        {
            Configuration.LazyLoadingEnabled = true;

            Configuration.ProxyCreationEnabled = true;

            Configuration.UseDatabaseNullSemantics = true;
        }

        protected BaseContext(DbConnection connection)
        : base(connection, true)
        {
        }

        protected void OnModelCreating<IConfiguration>(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties()
                .Where(p => p.Name == IdConst)
                .Configure(p => p.IsKey());

            var typesConnectorToRegister = GetType().Assembly.GetTypes()
                                                 .Where(p => typeof(IConfiguration).IsAssignableFrom(p))
                                                 .ToList();

            typesConnectorToRegister.ForEach(type => modelBuilder.Configurations.Add((dynamic)Activator.CreateInstance(type)));

            base.OnModelCreating(modelBuilder);
        }
    }
}
