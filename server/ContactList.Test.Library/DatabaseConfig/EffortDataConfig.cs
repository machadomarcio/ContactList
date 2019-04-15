using ContactList.Common.Contexts;
using Effort;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ContactList.Test.Library.DatabaseConfig
{
    public class EffortDataConfig
    {
        public void GenerateContexts(params Type[] contexts)
        {
            DatabaseFactory.CleanAllDbContexts();

            foreach (var type in contexts)
            {
                var connection = DbConnectionFactory.CreatePersistent(Guid.NewGuid().ToString());

                var context = (BaseContext)Activator.CreateInstance(type, connection);

                DatabaseFactory.AddContext(type, context);
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            DatabaseFactory.CleanAllDbContexts();
        }
    }
}
