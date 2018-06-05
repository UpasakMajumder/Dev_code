using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.Order;
using Kadena.Models;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.BusinessLogic.Factories
{
    public interface IOrderReportFactory
    {
        /// <summary>
        /// Create report from regular order
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        OrderReport Create(RecentOrderDto orderDto);

        /// <summary>
        /// Map typed report view to generic table view
        /// </summary>
        /// <param name="reportDto"></param>
        /// <returns></returns>
        TableView CreateTableView(OrderReportView reportDto);

        /// <summary>
        /// Map order report to typed report view
        /// </summary>
        /// <param name="orderReports"></param>
        /// <returns></returns>
        OrderReportView CreateReportView(IEnumerable<OrderReport> orderReports);
    }

    public class OrderReportFactory : IOrderReportFactory
    {
        private readonly IKenticoOrderProvider kenticoOrderProvider;
        private readonly IKenticoCustomerProvider kenticoCustomerProvider;
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
                    var defaultUrl = kenticoResources.GetSiteSettingsKey(Settings.KDA_OrderDetailUrl);
                    _orderDetailUrl = kenticoDocumentProvider.GetDocumentUrl(defaultUrl);
                }
                return _orderDetailUrl;
            }
        }

        public OrderReportFactory(IKenticoOrderProvider kenticoOrderProvider,
            IKenticoCustomerProvider kenticoCustomerProvider,
            IDateTimeFormatter dateTimeFormatter,
            IKenticoResourceService kenticoResources,
            IKenticoDocumentProvider kenticoDocumentProvider)
        {
            this.kenticoOrderProvider = kenticoOrderProvider ?? throw new ArgumentNullException(nameof(kenticoOrderProvider));
            this.kenticoCustomerProvider = kenticoCustomerProvider ?? throw new ArgumentNullException(nameof(kenticoCustomerProvider));
            this.dateTimeFormatter = dateTimeFormatter ?? throw new ArgumentNullException(nameof(dateTimeFormatter));
            this.kenticoResources = kenticoResources ?? throw new ArgumentNullException(nameof(kenticoResources));
            this.kenticoDocumentProvider = kenticoDocumentProvider ?? throw new ArgumentNullException(nameof(kenticoDocumentProvider));
        }
        public OrderReport Create(RecentOrderDto orderDto) => 
            new OrderReport
            {
                Items = orderDto.Items.Select(it => new ReportLineItem
                {
                    Name = it.Name,
                    Price = it.UnitPrice,
                    Quantity = it.Quantity,
                    SKU = it.SKUNumber,
                    TrackingNumber = it.TrackingNumber,
                }).ToList(),
                Number = orderDto.Id,
                OrderingDate = orderDto.CreateDate,
                ShippingDate = orderDto.ShippingDate,
                Site = orderDto.SiteName,
                Status = FormatOrderStatus(orderDto.Status),
                Url = FormatDetailUrl(orderDto),
                User = FormatCustomer(kenticoCustomerProvider.GetCustomer(orderDto.CustomerId))
            };

        public OrderReportView CreateReportView(IEnumerable<OrderReport> orderReports) =>
            new OrderReportView
            {
                Items = orderReports
                    .SelectMany(o => o.Items.Select(it => new OrderReportViewItem
                    {
                        Url = o.Url,
                        Site = o.Site,
                        Number = o.Number,
                        OrderingDate = FormatDate(o.OrderingDate),
                        User = o.User,
                        Name = it.Name,
                        SKU = it.SKU,
                        Quantity = it.Quantity,
                        Price = it.Price,
                        Status = o.Status,
                        ShippingDate = FormatDate(o.ShippingDate),
                        TrackingNumber = it.TrackingNumber
                    }))
                    .ToArray()
            };
        
        public TableView CreateTableView(OrderReportView reportDto) => 
            new TableView
            {
                Rows = reportDto.Items.Select(it => new TableRow
                {
                    Url = it.Url,
                    Items = new object[]
                    {
                        it.Site,
                        it.Number,
                        it.OrderingDate,
                        it.User,
                        it.Name,
                        it.SKU,
                        it.Quantity,
                        it.Price,
                        it.Status,
                        it.ShippingDate,
                        it.TrackingNumber
                    }
                }).ToArray(),
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

        public string FormatDetailUrl(RecentOrderDto order) => 
            $"{OrderDetailUrl}?orderID={order.Id}";

        public string FormatOrderStatus(string status) => 
            kenticoOrderProvider.MapOrderStatus(status);

        public string FormatDate(DateTime? date) =>
            date.HasValue
                ? dateTimeFormatter.Format(date.Value)
                : string.Empty;
    }
}
