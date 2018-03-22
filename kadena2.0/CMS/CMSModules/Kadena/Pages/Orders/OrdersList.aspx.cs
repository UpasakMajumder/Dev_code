using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.EventLog;
using CMS.SiteProvider;
using Kadena.Container.Default;
using Kadena.Helpers.Data;
using Kadena2.MicroserviceClients.Contracts;
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
            var client = DIContainer.Resolve<IOrderViewClient>();
            var initialPageNumber = 1;
            var initialQuantity = 1;
            // First request to get total count of records.
            var data = client.GetOrders(SiteContext.CurrentSiteName, initialPageNumber, initialQuantity).Result;

            if (data?.Success ?? false)
            {
                if ((data.Payload.Orders?.Count() ?? 0) > 0)
                {
                    // Second request to get all records.
                    data = client.GetOrders(SiteContext.CurrentSiteName, initialPageNumber, data.Payload.TotalCount).Result;

                    if (data?.Success ?? false)
                    {
                        var customers = BaseAbstractInfoProvider.GetInfosByIds(CustomerInfo.OBJECT_TYPE, 
                            data.Payload.Orders.Select(o => o.CustomerId));

                        // Unigrid accept only DataSet as source type.
                        grdOrders.DataSource = data.Payload.Orders
                            .Select(o =>
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
                            })
                            .ToDataSet();
                    }
                }
            }

            if(!(data?.Success ?? false))
            {
                var exc = new InvalidOperationException(data?.ErrorMessages);
                EventLogProvider.LogException("OrdersList - Load", "EXCEPTION", exc);
            }
        }
    }
}