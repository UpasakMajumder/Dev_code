using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.SiteProvider;
using Kadena2.MicroserviceClients.Clients;
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
            var url = SettingsKeyInfoProvider.GetValue("KDA_OrdersBySiteUrl");
            var client = new OrderViewClient();
            var data = client.GetOrders(url, SiteContext.CurrentSiteName, 1, 1).Result;
            data = client.GetOrders(url, SiteContext.CurrentSiteName, 1, data.Payload.TotalCount).Result;
            var customers = BaseAbstractInfoProvider.GetInfosByIds(CustomerInfo.OBJECT_TYPE, data.Payload.Orders.Select(o => o.CustomerId));
            grdOrders.DataSource = ToDataSet(data.Payload.Orders.Select(o=> new {
                o.Id,
                o.Status,
                o.TotalPrice, o.CreateDate,
                CustomerName = $"{customers[o.CustomerId]?.GetStringValue("CustomerFirstName", string.Empty)} {customers[o.CustomerId]?.GetStringValue("CustomerLastName", string.Empty)}" })
                .ToList());
        }

        private static DataSet ToDataSet<T>(IList<T> list)
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
}