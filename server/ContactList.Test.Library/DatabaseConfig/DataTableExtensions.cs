using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ContactList.Test.Library.DatabaseConfig
{
    public static class DataTableExtensions
    {
        public static DataRow FirstRow(this DataTable tbl)
        {
            return tbl.Rows[0];
        }

        public static DataRow LastRow(this DataTable tbl)
        {
            var count = tbl.Rows.Count;
            return tbl.Rows[count - 1];
        }

        public static DataTable Action(this DataRow row, Action<DataRow> action)
        {
            action(row);

            return row.Table;
        }

        /// <summary>
        /// Get table element with given column names and row indices
        /// </summary>
        public static object GetElementValue(this DataTable tbl, string columnName, int RowInd)
        {
            return tbl.Rows[RowInd][columnName];
        }

        public static DataRow GetRowByValue(this DataTable tbl, string columnName, object value)
        {
            return GetRowsByValue(tbl, columnName, value).FirstOrDefault();
        }

        public static List<DataRow> GetRowsByValue(this DataTable tbl, string columnName, object value)
        {
            return tbl.Rows.Cast<DataRow>().Where(row => row[columnName].Equals(value)).ToList();
        }

        public static List<DataRow> GetRowsOrderBy(this DataTable tbl, string columnName)
        {
            return tbl.Rows.Cast<DataRow>().OrderBy(row => row[columnName]).ToList();
        }
    }
}
