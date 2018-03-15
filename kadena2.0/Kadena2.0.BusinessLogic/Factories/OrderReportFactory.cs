using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.Order;
using Kadena.Models;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.BusinessLogic.Factories
{
    public interface IOrderReportFactory
    {
        OrderReport Create(RecentOrderDto orderDto);
        TableView CreateTableView(IEnumerable<OrderReport> orderReports);
    }

    public class OrderReportFactory : IOrderReportFactory
    {
        private readonly IKenticoOrderProvider kenticoOrderProvider;
        private readonly IKenticoUserProvider kenticoUserProvider;
        private readonly IDateTimeFormatter dateTimeFormatter;
        private readonly IKenticoResourceService kenticoResources;
        private readonly IKenticoDocumentProvider kenticoDocumentProvider;
        private string _orderDetailUrl = string.Empty;
        public string OrderDetailUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_orderDetailUrl))
                {
                    var defaultUrl = kenticoResources.GetSettingsKey(Settings.KDA_OrderDetailUrl);
                    _orderDetailUrl = kenticoDocumentProvider.GetDocumentUrl(defaultUrl);
                }
                return _orderDetailUrl;
            }
        }

        public OrderReportFactory(IKenticoOrderProvider kenticoOrderProvider,
            IKenticoUserProvider kenticoUserProvider,
            IDateTimeFormatter dateTimeFormatter,
            IKenticoResourceService kenticoResources,
            IKenticoDocumentProvider kenticoDocumentProvider)
        {
            this.kenticoOrderProvider = kenticoOrderProvider;
            this.kenticoUserProvider = kenticoUserProvider;
            this.dateTimeFormatter = dateTimeFormatter;
            this.kenticoResources = kenticoResources;
            this.kenticoDocumentProvider = kenticoDocumentProvider;
        }
        public OrderReport Create(RecentOrderDto orderDto)
        {
            return new OrderReport
            {
                Items = orderDto.Items.Select(it => new ReportLineItem
                {
                    Name = it.Name,
                    Price = it.UnitPrice,
                    Quantity = it.Quantity,
                    SKU = it.SKUNumber
                }).ToList(),
                Number = orderDto.Id,
                OrderingDate = orderDto.CreateDate,
                ShippingDate = orderDto.ShippingDate,
                Site = orderDto.SiteName,
                Status = FormatOrderStatus(orderDto.Status),
                TrackingNumber = orderDto.TrackingNumber,
                Url = FormatDetailUrl(orderDto),
                User = FormatCustomer(kenticoUserProvider.GetCustomer(orderDto.CustomerId))
            };
        }

        public TableView CreateTableView(IEnumerable<OrderReport> orderReports)
        {
            var rows = orderReports.SelectMany(o => o.Items.Select(it => new TableRow
            {
                Url = o.Url,
                Items = new object[]
                {
                    o.Site,
                    o.Number,
                    dateTimeFormatter.Format(o.OrderingDate),
                    o.User,
                    it.Name,
                    it.SKU,
                    it.Quantity,
                    it.Price,
                    o.Status,
                    o.ShippingDate.HasValue ? dateTimeFormatter.Format(o.ShippingDate.Value) : string.Empty,
                    o.TrackingNumber
                }
            })).ToArray();
            return new TableView
            {
                Rows = rows,
                Headers = new[] 
                {
                    kenticoResources.GetResourceString("Kadena.OrdersReport.Table.Site"),
                    kenticoResources.GetResourceString("Kadena.OrdersReport.Table.Number"),
                    kenticoResources.GetResourceString("Kadena.OrdersReport.Table.OrderDate"),
                    kenticoResources.GetResourceString("Kadena.OrdersReport.Table.User"),
                    kenticoResources.GetResourceString("Kadena.OrdersReport.Table.Name"),
                    kenticoResources.GetResourceString("Kadena.OrdersReport.Table.SKU"),
                    kenticoResources.GetResourceString("Kadena.OrdersReport.Table.Quantity"),
                    kenticoResources.GetResourceString("Kadena.OrdersReport.Table.Price"),
                    kenticoResources.GetResourceString("Kadena.OrdersReport.Table.Status"),
                    kenticoResources.GetResourceString("Kadena.OrdersReport.Table.ShippingDate"),
                    kenticoResources.GetResourceString("Kadena.OrdersReport.Table.TrackingNumber")
                }
            };
        }

        public string FormatCustomer(Customer customer)
        {
            if (customer == null)
            {
                return string.Empty;
            }

            var name = $"{customer.FirstName} {customer.LastName}";
            if (!string.IsNullOrWhiteSpace(name))
            {
                return name;
            }

            return customer.Email;
        }

        public string FormatDetailUrl(RecentOrderDto order)
        {
            return $"{OrderDetailUrl}?orderID={order.Id}";
        }

        public string FormatOrderStatus(string status)
        {
            return kenticoOrderProvider.MapOrderStatus(status);
        }
    }
}
