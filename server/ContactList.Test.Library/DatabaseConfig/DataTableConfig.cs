using System;
using System.Data;
using System.IO;

namespace ContactList.Test.Library.DatabaseConfig
{
    public class DataTableConfig : IDisposable
    {
        protected DbUnitTest _dbContext;
        private static string nameSchema = "Schema";
        private static string connectionString = "";

        public DataTable Table_Contact => _dbContext.GetTable("dbo.Person");
        public DataTable Table_ContactValue => _dbContext.GetTable("dbo.ContactValue");
        

        public DataTableConfig()
        {
            _dbContext = new DbUnitTest(nameSchema, connectionString);
        }

        public void InsertTestData(params string[] dataFileNames)
        {
            _dbContext.InsertTestData(dataFileNames);
        }

        public void InsertTestData(Stream xml)
        {
            _dbContext.InsertTestData(xml);
        }

        public void ExecuteScript(params string[] dataFileNames)
        {
            _dbContext.ExecuteScript(dataFileNames);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}