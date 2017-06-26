using CMS.Ecommerce.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Kadena.CMSModules.Kadena.Pages.Orders
{
    public partial class OrdersList : CMSEcommercePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var source = (new SomeDto[] {
                new SomeDto{ OrderNumber = "412412421" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421124124" },
                new SomeDto{ OrderNumber = "412412421qwfq" }
                }).ToList();
            grdOrders.DataSource = ToDataSet(source);
        }

        public static DataSet ToDataSet<T>(IList<T> list)
        {
            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            //add a column to table for each public property on T   
            foreach (var propInfo in elementType.GetProperties())
            {
                t.Columns.Add(propInfo.Name, propInfo.PropertyType);
            }

            //go through each property on T and add each value to the table   
            foreach (T item in list)
            {
                DataRow row = t.NewRow(); foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null);
                }
                t.Rows.Add(row);
            }
            return ds;
        }
    }

    public class SomeDto
    {
        public string OrderNumber { get; set; }
    }
}