using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kadena.Old_App_Code.Kadena.Imports
{
    public class ImportHelper
    {
        public static Func<string[], T> GetImportMapper<T>(string[] header)
        {
            var properties = GetHeaderProperties<T>();
            var columnIndexToPropertyMap = new Dictionary<int, PropertyInfo>();
            for (int colIndex = 0; colIndex < header.Length; colIndex++)
            {
                for (int propIndex = 0; propIndex < properties.Count; propIndex++)
                {
                    if (string.Compare(properties[propIndex].Key, header[colIndex], ignoreCase: true) == 0)
                    {
                        columnIndexToPropertyMap[colIndex] = properties[propIndex].Value;
                        break;
                    }
                }
            }

            return (row) =>
            {
                var item = Activator.CreateInstance<T>();
                for (int i = 0; i < row.Length; i++)
                {
                    PropertyInfo property;
                    if (columnIndexToPropertyMap.TryGetValue(i, out property))
                    {
                        property.SetValue(item, row[i]);
                    }
                }

                return item;
            };
        }

        public static ExcelType GetExcelTypeFromFileName(string fileName)
        {
            if (fileName.EndsWith(".xlsx", StringComparison.InvariantCultureIgnoreCase))
            {
                return ExcelType.Xlsx;
            }
            else
            {
                return ExcelType.Xls;
            }
        }

        public static List<KeyValuePair<string, PropertyInfo>> GetHeaderProperties<T>()
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var namedProperties = properties.Select(p => new { Property = p, HeaderInfo = p.GetCustomAttributes(inherit: false).FirstOrDefault(a => a is HeaderAttribute) as HeaderAttribute })
                .Where(p => p.HeaderInfo != null)
                .OrderBy(p => p.HeaderInfo.Order)
                .Select(p => new KeyValuePair<string, PropertyInfo>(p.HeaderInfo.Title, p.Property))
                .ToList();
            return namedProperties;
        }
    }
}