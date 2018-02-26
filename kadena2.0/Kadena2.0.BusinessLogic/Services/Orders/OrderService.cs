using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IKenticoResourceService kenticoResources;
        private readonly IKenticoSiteProvider kenticoSiteProvider;
        private readonly IDateTimeFormatter dateTimeFormatter;
        private readonly IOrderViewClient orderViewClient;
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

        public OrderService(IKenticoResourceService kenticoResources,
            IKenticoSiteProvider kenticoSiteProvider,
            IDateTimeFormatter dateTimeFormatter,
            IOrderViewClient orderViewClient)
        {
            this.kenticoResources = kenticoResources;
            this.kenticoSiteProvider = kenticoSiteProvider;
            this.dateTimeFormatter = dateTimeFormatter;
            this.orderViewClient = orderViewClient;
        }

        public Task<PagedData<Order>> GetOrders(int page, OrderFilter filter)
        {
            var currentSite = kenticoSiteProvider.GetCurrentSiteCodeName();
            return GetOrdersForSite(currentSite, page, filter);
        }

        public Task<PagedData<Order>> GetOrdersForSite(string site, int page, OrderFilter filter)
        {
            ValidatePageNumber(page);
            ValidateFilter(filter);

            OrderFilter.SortFields sort;
            var sortSpecified = filter.TryParseSort(out sort);
            var sortProperty = sortSpecified ? sort.Property : null;
            var sortDesc = sortSpecified ? sort.Direction == OrderFilter.SortDirection.DESC : false;

            orderViewClient.GetOrders(site, null, page, OrdersPerPage, filter.FromDate, filter.ToDate, sortProperty, sortDesc, null, null);

            return null;
        }

        public TableView ConvertOrdersToView(PagedData<Order> orders)
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

        public Task<FileResult> GetOrdersExport(string format, OrderFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<FileResult> GetOrdersExportForSite(string site, string format, OrderFilter filter)
        {
            throw new NotImplementedException();
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
