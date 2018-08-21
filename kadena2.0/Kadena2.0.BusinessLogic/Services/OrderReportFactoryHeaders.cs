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
            return new []
            {
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
                _kenticoResourceService.GetResourceString("Kadena.OrdersReport.Table.TrackingNumber")
            };
        }

        public string[] GetCodeNameHeaders()
        {
            return new[]
            {
                "Site",
                "orderNumber",
                "CreateDate",
                "User",
                "Name",
                "SKU",
                "Quantity",
                "Price",
                "Status",
                "ShippingDate",
                "TrackingNumber"
            };
        }
    }
}
