using System;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class OrderReportFactoryHeaders : IOrderReportFactoryHeaders
    {
        private readonly IKenticoResourceService _kenticoResourceService;

        public OrderReportFactoryHeaders(IKenticoResourceService kenticoResourceService)
        {
            _kenticoResourceService = kenticoResourceService ?? throw new ArgumentNullException(nameof(kenticoResourceService));
        }

        public string[] GetDisplayNameHeaders()
        {
            // do not change order in this array
            return new []
            {
                _kenticoResourceService.GetResourceString("Kadena.OrdersReport.Table.LineNumber"),
                _kenticoResourceService.GetResourceString("Kadena.OrdersReport.Table.Site"),
                _kenticoResourceService.GetResourceString("Kadena.OrdersReport.Table.Number"),
                _kenticoResourceService.GetResourceString("Kadena.OrdersReport.Table.OrderDate"),
                _kenticoResourceService.GetResourceString("Kadena.OrdersReport.Table.User"),
                _kenticoResourceService.GetResourceString("Kadena.OrdersReport.Table.Name"),
                _kenticoResourceService.GetResourceString("Kadena.OrdersReport.Table.SKU"),
                _kenticoResourceService.GetResourceString("Kadena.OrdersReport.Table.Quantity"),
                _kenticoResourceService.GetResourceString("Kadena.OrdersReport.Table.Price"),
                _kenticoResourceService.GetResourceString("Kadena.OrdersReport.Table.Status"),
                _kenticoResourceService.GetResourceString("Kadena.OrdersReport.Table.ShippingDate"),
                _kenticoResourceService.GetResourceString("Kadena.OrdersReport.Table.TrackingNumber"),
                _kenticoResourceService.GetResourceString("Kadena.OrdersReport.Table.ShippedQuantity"),
                _kenticoResourceService.GetResourceString("Kadena.OrdersReport.Table.ShippingMethod"),
                _kenticoResourceService.GetResourceString("Kadena.OrdersReport.Table.TrackingInfoId")
            };
        }

        public string[] GetCodeNameHeaders()
        {
            // do not change order in this array
            return new[]
            {
                "lineNumber",
                "site",
                "orderNumber",
                "createDate",
                "user",
                "name",
                "sku",
                "quantity",
                "price",
                "status",
                "shippingDate",
                "trackingNumber",
                "shippedQuantity",
                "shippingMethod",
                "trackingInfoId"
            };
        }
    }
}
