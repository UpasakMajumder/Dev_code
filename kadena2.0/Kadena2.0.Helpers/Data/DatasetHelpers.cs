using System.Collections.Generic;
using System.Data;

namespace Kadena.Helpers.Data
{
    public static class DatasetHelpers
    {
        public static DataSet ToDataSet<T>(this IEnumerable<T> items)
        {
            var properties = typeof(T).GetProperties();
            var table = new DataTable();

            // Add a column to table for each public property on T   
            foreach (var propInfo in properties)
            {
                table.Columns.Add(propInfo.Name, propInfo.PropertyType);
            }

            //go through each property on T and add each value to the table   
            foreach (T item in items)
            {
                DataRow row = table.NewRow();
                foreach (var propInfo in properties)
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null);
                }
                table.Rows.Add(row);
            }
            return new DataSet { Tables = { table } };
        }
    }
}
