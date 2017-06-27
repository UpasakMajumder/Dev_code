﻿using CMS.DataEngine;
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
            
            // First request to get total count of records.
            var data = client.GetOrders(url, SiteContext.CurrentSiteName, 1, 1).Result;

            // Second request to get all records.
            data = client.GetOrders(url, SiteContext.CurrentSiteName, 1, data.Payload.TotalCount).Result;
            var customers = BaseAbstractInfoProvider.GetInfosByIds(CustomerInfo.OBJECT_TYPE, 
                data.Payload.Orders.Select(o => o.CustomerId));

            // Unigrid accept only DataSet as source type.
            grdOrders.DataSource = ToDataSet(
                data.Payload.Orders.Select(o =>
                {
                    var customer = customers[o.CustomerId] as CustomerInfo;
                    return new
                    {
                        o.Id,
                        o.Status,
                        o.TotalPrice,
                        o.CreateDate,
                        CustomerName = customer != null ?
                                            $"{customer.CustomerFirstName} {customer.CustomerLastName}"
                                            : string.Empty
                    };
                }));
        }

        private static DataSet ToDataSet<T>(IEnumerable<T> list)
        {
            var properties = typeof(T).GetProperties();
            var table = new DataTable();

            // Add a column to table for each public property on T   
            foreach (var propInfo in properties)
            {
                table.Columns.Add(propInfo.Name, propInfo.PropertyType);
            }

            //go through each property on T and add each value to the table   
            foreach (T item in list)
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