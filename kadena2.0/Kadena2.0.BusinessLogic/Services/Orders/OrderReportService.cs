using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.BusinessLogic.Factories;
using Kadena.Dto.Order;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderReportService : IOrderReportService
    {
        private readonly IKenticoResourceService kenticoResources;
        private readonly IKenticoSiteProvider kenticoSiteProvider;
        private readonly IOrderViewClient orderViewClient;
        private readonly IConvert xlsxConvert;
        private readonly IOrderReportFactory orderReportFactory;
        private readonly IKenticoLogger logger;
        private readonly IMapper mapper;
        public const int DefaultCountOfOrdersPerPage = 20;
        public const int FirstPageNumber = 1;

        public const string SortableByOrderDate = "CreateDate";

        private int _ordersPerPage;
        public int OrdersPerPage
        {
            get
            {
                if (_ordersPerPage == 0)
                {
                    int.TryParse(kenticoResources.GetSiteSettingsKey(Settings.KDA_RecentOrdersPageCapacity), out _ordersPerPage);
                    if (_ordersPerPage == 0)
                    {
                        return DefaultCountOfOrdersPerPage;
                    }
                }
                return _ordersPerPage;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException($"Given value {value} is invalid. Value must be greater than 0");
                }

                _ordersPerPage = value;
            }
        }

        public OrderReportService(IKenticoResourceService kenticoResources,
            IKenticoSiteProvider kenticoSiteProvider,
            IOrderViewClient orderViewClient,
            IConvert xlsxConvert,
            IOrderReportFactory orderReportFactory,
            IKenticoLogger logger,
            IMapper mapper)
        {
            this.kenticoResources = kenticoResources ?? throw new ArgumentNullException(nameof(kenticoResources));
            this.kenticoSiteProvider = kenticoSiteProvider ?? throw new ArgumentNullException(nameof(kenticoSiteProvider));
            this.orderViewClient = orderViewClient ?? throw new ArgumentNullException(nameof(orderViewClient));
            this.xlsxConvert = xlsxConvert ?? throw new ArgumentNullException(nameof(xlsxConvert));
            this.orderReportFactory = orderReportFactory ?? throw new ArgumentNullException(nameof(orderReportFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PagedData<OrderReportViewItem>> GetOrdersForSite(string site, int page, OrderFilter filter)
        {
            var orderFilter = CreateOrderListFilter(filter, site, page);
            var orders = await GetOrders(orderFilter);

            return new PagedData<OrderReportViewItem>
            {
                Pagination = orders.Pagination,
                Data = orderReportFactory.CreateReportView(orders.Data).ToList()
            };
        }

        public async Task<TableView> ConvertOrdersToView(int page, OrderFilter filter)
        {
            var currentSite = kenticoSiteProvider.GetCurrentSiteCodeName();
            var orders = await GetOrdersForSite(currentSite, page, filter);

            var table = orderReportFactory.CreateTableView(orders.Data);
            table.Pagination = orders.Pagination;

            return table;
        }

        public Task<FileResult> GetOrdersExport(OrderFilter filter)
        {
            var currentSite = kenticoSiteProvider.GetCurrentSiteCodeName();
            return GetOrdersExportForSite(currentSite, filter);
        }

        public async Task<FileResult> GetOrdersExportForSite(string site, OrderFilter filter)
        {
            var orderFilter = CreateOrderListFilter(filter, site);
            var orders = await GetOrders(orderFilter);

            var report = orderReportFactory.CreateReportView(orders.Data);
            var tableView = orderReportFactory.CreateTableView(report.ToList());

            var fileResult = new FileResult
            {
                Data = xlsxConvert.GetBytes(tableView),
                Name = "export.xlsx",
                Mime = ContentTypes.Xlsx
            };

            return fileResult;
        }

        private async Task<PagedData<RecentOrderDto>> GetOrders(OrderListFilter orderFilter)
        {
            var ordersDto = await orderViewClient.GetOrders(orderFilter).ConfigureAwait(false);
            if (!ordersDto.Success)
            {
                logger.LogError(nameof(OrderReportService), ordersDto.ErrorMessages);
                return PagedData<RecentOrderDto>.Empty();
            }

            var orders = ordersDto.Payload?.Orders ?? new List<RecentOrderDto>();

            if (orders.Count() > 0)
            {
                var pagesCount = ordersDto.Payload.TotalCount / OrdersPerPage;
                if (ordersDto.Payload.TotalCount % OrdersPerPage > 0)
                {
                    pagesCount++;
                }

                return new PagedData<RecentOrderDto>
                {
                    Pagination = new Pagination
                    {
                        CurrentPage = orderFilter.PageNumber ?? 0,
                        RowsCount = ordersDto.Payload.TotalCount,
                        RowsOnPage = OrdersPerPage,
                        PagesCount = pagesCount
                    },
                    Data = orders.ToList()
                };
            }

            return new PagedData<RecentOrderDto>
            {
                Data = orders.ToList()
            };
        }

        private OrderListFilter CreateOrderListFilter(OrderFilter filter, string site, int page)
        {
            ValidatePageNumber(page);

            var orderFilter = CreateOrderListFilter(filter, site);
            orderFilter.PageNumber = page;
            orderFilter.ItemsPerPage = OrdersPerPage;

            return orderFilter;
        }

        private OrderListFilter CreateOrderListFilter(OrderFilter filter, string site)
        {
            ValidateFilter(filter);

            var orderFilter = new OrderListFilter
            {
                SiteName = site,
                DateFrom = filter.FromDate,
                DateTo = filter.ToDate
            };

            if (orderFilter.DateTo != null)
            {
                orderFilter.DateTo = orderFilter.DateTo.Value.AddDays(1);
            }

            OrderFilter.OrderByFields sort;
            if (filter.TryParseOrderByExpression(out sort))
            {
                orderFilter.OrderBy = sort.Property;
                orderFilter.OrderByDescending = sort.Direction == OrderFilter.OrderByDirection.DESC;
            }
            return orderFilter;
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
            if (!string.IsNullOrWhiteSpace(filter.OrderByExpression))
            {
                OrderFilter.OrderByFields sortFields;
                if (!filter.TryParseOrderByExpression(out sortFields))
                {
                    throw new ArgumentException($"Invalid value for filter.Sort '{filter.OrderByExpression}'", nameof(filter));
                }

                if (sortFields.Property != SortableByOrderDate)
                {
                    throw new ArgumentException($"Invalid value for filter.Sort. Sorting by '{sortFields.Property}' is not supported", nameof(filter));
                }
            }

            var isInvalidRange = (filter.FromDate != null && filter.ToDate != null) && filter.FromDate > filter.ToDate;
            if (isInvalidRange)
            {
                throw new ArgumentException($"Invalid values for date. 'From date' must be smaller than 'To date'", nameof(filter));
            }
        }
    }
}
