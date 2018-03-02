using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Dto.Order;
using Kadena.Models;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderReportService : IOrderReportService
    {
        private readonly IKenticoResourceService kenticoResources;
        private readonly IKenticoSiteProvider kenticoSiteProvider;
        private readonly IDateTimeFormatter dateTimeFormatter;
        private readonly IOrderViewClient orderViewClient;
        private readonly IKenticoUserProvider kenticoUserProvider;
        private readonly IKenticoDocumentProvider kenticoDocumentProvider;
        private readonly IKenticoOrderProvider kenticoOrderProvider;
        public const int DefaultCountOfOrdersPerPage = 20;
        public const int FirstPageNumber = 1;

        public const string SortableByOrderDate = "OrderDate";

        private int _ordersPerPage;
        public int OrdersPerPage
        {
            get
            {
                int.TryParse(kenticoResources.GetSettingsKey(Settings.KDA_RecentOrdersPageCapacity), out _ordersPerPage);
                if (_ordersPerPage == 0)
                {
                    return DefaultCountOfOrdersPerPage;
                }
                return _ordersPerPage;
            }
        }

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

        public OrderReportService(IKenticoResourceService kenticoResources,
            IKenticoSiteProvider kenticoSiteProvider,
            IDateTimeFormatter dateTimeFormatter,
            IOrderViewClient orderViewClient,
            IKenticoUserProvider kenticoUserProvider,
            IKenticoDocumentProvider kenticoDocumentProvider,
            IKenticoOrderProvider kenticoOrderProvider)
        {
            this.kenticoResources = kenticoResources;
            this.kenticoSiteProvider = kenticoSiteProvider;
            this.dateTimeFormatter = dateTimeFormatter;
            this.orderViewClient = orderViewClient;
            this.kenticoUserProvider = kenticoUserProvider;
            this.kenticoDocumentProvider = kenticoDocumentProvider;
            this.kenticoOrderProvider = kenticoOrderProvider;
        }

        public virtual Task<PagedData<OrderReport>> GetOrders(int page, OrderFilter filter)
        {
            var currentSite = kenticoSiteProvider.GetCurrentSiteCodeName();
            return GetOrdersForSite(currentSite, page, filter);
        }

        public virtual async Task<PagedData<OrderReport>> GetOrdersForSite(string site, int page, OrderFilter filter)
        {
            ValidatePageNumber(page);
            ValidateFilter(filter);

            OrderFilter.SortFields sort;
            var sortSpecified = filter.TryParseSort(out sort);
            var sortProperty = sortSpecified ? sort.Property : null;
            var sortDesc = sortSpecified ? sort.Direction == OrderFilter.SortDirection.DESC : false;

            int? customerId = null;
            int? campaign = null;
            string orderType = null;

            var orders = await orderViewClient.GetOrders(site, customerId, page, OrdersPerPage, filter.FromDate, filter.ToDate, sortProperty, sortDesc, campaign, orderType);
            var pagesCount = orders.Payload.TotalCount / OrdersPerPage;
            if (orders.Payload.TotalCount % OrdersPerPage > 0)
            {
                pagesCount++;
            }

            return new PagedData<OrderReport>
            {
                Pagination = new Pagination
                {
                    CurrentPage = page,
                    RowsCount = orders.Payload.TotalCount,
                    RowsOnPage = OrdersPerPage,
                    PagesCount = pagesCount
                },
                Data = orders.Payload.Orders.Select(o => new OrderReport
                {
                    Items = o.Items.Select(it => new ReportLineItem
                    {
                        Name = it.Name,
                        Price = it.Price,
                        Quantity = it.Quantity,
                        SKU = it.SKU
                    }).ToList(),
                    Number = o.Id,
                    OrderingDate = o.CreateDate,
                    ShippingDate = o.ShippingDate,
                    Site = o.SiteName,
                    Status = FormatOrderStatus(o.Status),
                    TrackingNumber = o.TrackingNumber,
                    Url = FormatDetailUrl(o),
                    User = FormatCustomer(kenticoUserProvider.GetCustomer(o.CustomerId))
                }).ToList()
            };
        }

        public TableView ConvertOrdersToView(PagedData<OrderReport> orders)
        {
            if (orders == null)
            {
                throw new ArgumentNullException(nameof(orders));
            }

            var rows = orders.Data?
                .SelectMany(o => o.Items.Select(it => new TableRow
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
                }))
                .ToArray();

            var view = new TableView
            {
                Pagination = orders.Pagination,
                Rows = rows
            };
            return view;
        }

        public virtual Task<FileResult> GetOrdersExport(string format, OrderFilter filter)
        {
            throw new NotImplementedException();
        }

        public virtual Task<FileResult> GetOrdersExportForSite(string site, string format, OrderFilter filter)
        {
            throw new NotImplementedException();
        }

        public string FormatCustomer(Customer customer)
        {
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

        private void ValidatePageNumber(int page)
        {
            if (page < FirstPageNumber)
            {
                throw new ArgumentException($"Page must be >= {FirstPageNumber}", nameof(page));
            }
        }

        private void ValidateFilter(OrderFilter filter)
        {
            if (string.IsNullOrWhiteSpace(filter.Sort))
            {
                return;
            }

            OrderFilter.SortFields sortFields;
            if (!filter.TryParseSort(out sortFields))
            {
                throw new ArgumentException($"Invalid value for filter.Sort '{filter.Sort}'", nameof(filter));
            }

            if (sortFields.Property != SortableByOrderDate)
            {
                throw new ArgumentException($"Invalid value for filter.Sort. Sorting by '{sortFields.Property}' is not supported", nameof(filter));
            }
        }
    }
}
