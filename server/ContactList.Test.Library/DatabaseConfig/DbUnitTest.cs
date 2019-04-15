using ContactList.Common.Contexts;
using ContactList.Common.Extensions;
using NDbUnit.Core;
using NDbUnit.Core.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace ContactList.Test.Library.DatabaseConfig
{
    public class DbUnitTest
    {
        protected INDbUnitTest _db;
        protected string _xmlSchemaPath;

        public DbUnitTest(string xmlSchema, string connectionString, string[] autoIncrementColumns = null)
        {
            _db = new SqlDbUnitTest(connectionString);

            string pathSchema = CreateSchemaFromDataBase(xmlSchema, connectionString, autoIncrementColumns);

            _db.ReadXmlSchema(pathSchema);

            _xmlSchemaPath = pathSchema;

            _db.PerformDbOperation(DbOperationFlag.DeleteAll);

            DatabaseFactory.CleanContexts();
        }

        public string CreateSchemaFromDataBase(string nameSchema, string connectionString, string[] autoIncrementColumns)
        {
            var dataSet = new DataSet(nameSchema);

            List<string> tables = new List<string>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var sql = "SELECT TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var schema = reader.GetString(0);
                            var table = reader.GetString(1);
                            tables.Add(schema == "dbo" ? table : $"{schema}.{table}");
                        }
                    }
                }

                foreach (var table in tables)
                {
                    using (var sqlAdapter = new SqlDataAdapter($"SELECT * FROM {table}", connection))
                    {
                        sqlAdapter.Fill(dataSet, table);
                    }
                }
            }

            SetAutoIncrementColumns(dataSet, autoIncrementColumns);

            var pathSchema = $@"C:\Schemas\{nameSchema}.xsd";

            var xmlns = $"http://tempuri.org/{nameSchema}.xsd";

            XmlDocument xmlSchema = new XmlDocument();
            xmlSchema.LoadXml(dataSet.GetXmlSchema());

            xmlSchema.DocumentElement.SetAttribute("xmlns", xmlns);
            xmlSchema.DocumentElement.SetAttribute("xmlns:mstns", xmlns);
            xmlSchema.DocumentElement.SetAttribute("targetNamespace", xmlns);
            xmlSchema.DocumentElement.SetAttribute("xmlns:msprop", "urn:schemas-microsoft-com:xml-msprop");
            xmlSchema.DocumentElement.SetAttribute("attributeFormDefault", "qualified");
            xmlSchema.DocumentElement.SetAttribute("elementFormDefault", "qualified");

            XmlDeclaration xmlDeclaration = xmlSchema.GetOrCreateXmlDeclaration();
            xmlDeclaration.Encoding = Encoding.UTF8.WebName;

            try
            {
                xmlSchema.Save(pathSchema);
            }
            catch (Exception) { }

            return pathSchema;
        }

        private void SetAutoIncrementColumns(DataSet dataSet, string[] autoIncrementColumns)
        {
            autoIncrementColumns = autoIncrementColumns ?? new string[0];

            foreach (DataTable dataTable in dataSet.Tables)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    bool isColumnId = column.ColumnName == "Id" || autoIncrementColumns.Contains(column.ColumnName);
                    bool isInt = column.DataType == Type.GetType("System.Int32") || column.DataType == Type.GetType("System.Int64");

                    if (isColumnId && isInt)
                    {
                        column.AutoIncrement = true;
                        column.AutoIncrementSeed = 1;
                        column.AutoIncrementStep = 1;
                    }
                }
            }
        }

        public void InsertTestData(params string[] dataFileNames)
        {
            if (dataFileNames == null)
                return;
            foreach (var fileName in dataFileNames)
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException(Path.GetFullPath(fileName));
                _db.ReadXml(fileName);

                _db.PerformDbOperation(DbOperationFlag.InsertIdentity);
            }
        }

        public void InsertTestData(Stream xml)
        {
            if (xml == null)
                return;

            _db.ReadXml(xml);

            _db.PerformDbOperation(DbOperationFlag.InsertIdentity);
        }

        public void ExecuteScript(params string[] dataFileNames)
        {
            if (dataFileNames == null) return;

            foreach (var fileName in dataFileNames)
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException(Path.GetFullPath(fileName));
                _db.Scripts.AddSingle(fileName);
            }

            _db.ExecuteScripts();
        }

        public void Dispose()
        {
            _db.PerformDbOperation(DbOperationFlag.DeleteAll);
        }

        public DataTable GetTable(string table)
        {
            var tableFound = _db.GetDataSetFromDb().Tables[table];

            if (tableFound == null)
                throw new ApplicationException($"{ table } não está registrada no Schema: { _xmlSchemaPath }");

            return tableFound;
        }
    }
}