using AutoMapper;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.BusinessLogic.Factories;
using Kadena.Dto.Order;
using Kadena.Infrastructure.Contracts;
using Kadena.Infrastructure.FileConversion;
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
    public class OrderReportService : IOrderReportService
    {
        private readonly IKenticoResourceService kenticoResources;
        private readonly IKenticoSiteProvider kenticoSiteProvider;
        private readonly IOrderViewClient orderViewClient;
        private readonly IExcelConvert excelConvert;
        private readonly IOrderReportFactory orderReportFactory;
        private readonly IMapper mapper;
        public const int DefaultCountOfOrdersPerPage = 20;
        public const int FirstPageNumber = 1;

        public const string SortableByOrderDate = "CreateDate";

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

        public OrderReportService(IKenticoResourceService kenticoResources,
            IKenticoSiteProvider kenticoSiteProvider,
            IOrderViewClient orderViewClient,
            IExcelConvert excelConvert,
            IOrderReportFactory orderReportFactory,
            IMapper mapper)
        {
            this.kenticoResources = kenticoResources;
            this.kenticoSiteProvider = kenticoSiteProvider;
            this.orderViewClient = orderViewClient;
            this.excelConvert = excelConvert;
            this.orderReportFactory = orderReportFactory;
            this.mapper = mapper;
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

            OrderFilter.OrderByFields sort;
            var sortSpecified = filter.TryParseOrderByExpression(out sort);
            var sortProperty = sortSpecified ? sort.Property : null;
            var sortDesc = sortSpecified ? sort.Direction == OrderFilter.OrderByDirection.DESC : false;
            var orderFilter = new OrderListFilter
            {
                SiteName = site,
                PageNumber = page,
                ItemsPerPage = OrdersPerPage,
                DateFrom = filter.FromDate,
                DateTo = filter.ToDate,
                SortBy = sortProperty,
                SortDescending = sortDesc
            };

            var orders = await orderViewClient.GetOrders(orderFilter);
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
                Data = orders.Payload.Orders
                    .Select(o => orderReportFactory.Create(o))
                    .ToList()
            };
        }
        
        public TableView ConvertOrdersToView(PagedData<OrderReport> orders)
        {
            if (orders == null)
            {
                throw new ArgumentNullException(nameof(orders));
            }

            var view = orderReportFactory.CreateTableView(orders.Data);
            view.Pagination = orders.Pagination;

            return view;
        }

        public virtual Task<FileResult> GetOrdersExport(OrderFilter filter)
        {
            var currentSite = kenticoSiteProvider.GetCurrentSiteCodeName();
            return GetOrdersExportForSite(currentSite, filter);
        }

        public virtual async Task<FileResult> GetOrdersExportForSite(string site, OrderFilter filter)
        {
            ValidateFilter(filter);

            OrderFilter.OrderByFields sort;
            var sortSpecified = filter.TryParseOrderByExpression(out sort);
            var sortProperty = sortSpecified ? sort.Property : null;
            var sortDesc = sortSpecified ? sort.Direction == OrderFilter.OrderByDirection.DESC : false;
            var orderFilter = new OrderListFilter
            {
                SiteName = site,
                ItemsPerPage = OrdersPerPage,
                DateFrom = filter.FromDate,
                DateTo = filter.ToDate,
                SortBy = sortProperty,
                SortDescending = sortDesc
            };

            var orders = await orderViewClient.GetOrders(orderFilter);
            var ordersReport = orders.Payload.Orders.ToList()
                .Select(o => orderReportFactory.Create(o));
            var tableView = orderReportFactory.CreateTableView(ordersReport);

            var fileDataTable = mapper.Map<Table>(tableView);
            var fileData = excelConvert.Convert(fileDataTable);

            return new FileResult
            {
                Data = fileData,
                Name = "export.xlsx",
                Mime = ContentTypes.Xlsx
            };
        }

        private void ValidatePageNumber(int page)
        {
            if (page < FirstPageNumber)
            {
                throw new ArgumentException($"Page must be >= {FirstPageNumber}", nameof(page));
            }
        }

        public virtual void ValidateFilter(OrderFilter filter)
        {
            if (string.IsNullOrWhiteSpace(filter.OrderByExpression))
            {
                return;
            }

            OrderFilter.OrderByFields sortFields;
            if (!filter.TryParseOrderByExpression(out sortFields))
            {
                throw new ArgumentException($"Invalid value for filter.Sort '{filter.OrderByExpression}'", nameof(filter));
            }

            if (sortFields.Property != SortableByOrderDate)
            {
                throw new ArgumentException($"Invalid value for filter.Sort. Sorting by '{sortFields.Property}' is not supported", nameof(filter));
            }

            var isInvalidRange = (filter.FromDate != null && filter.ToDate != null) && filter.FromDate > filter.ToDate;
            if (isInvalidRange)
            {
                throw new ArgumentException($"Invalid values for date. 'From date' must be smaller than 'To date'", nameof(filter));
            }
        }
    }
}
