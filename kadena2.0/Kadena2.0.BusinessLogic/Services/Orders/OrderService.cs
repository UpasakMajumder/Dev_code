using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IKenticoResourceService kenticoResources;
        private readonly IDateTimeFormatter dateTimeFormatter;
        public const int DefaultOrdersPerPage = 20;

        private int _ordersPerPage;
        public int OrdersPerPage
        {
            get
            {
                int.TryParse(kenticoResources.GetSettingsKey(Settings.KDA_RecentOrdersPageCapacity), out _ordersPerPage);
                if (_ordersPerPage == 0)
                {
                    return DefaultOrdersPerPage;
                }
                return _ordersPerPage;
            }
        }

        public OrderService(IKenticoResourceService kenticoResources, IDateTimeFormatter dateTimeFormatter)
        {
            this.kenticoResources = kenticoResources;
            this.dateTimeFormatter = dateTimeFormatter;
        }

        public Task<PagedData<Order>> GetOrders(OrderFilter filter, int page)
        {
            throw new NotImplementedException();
        }

        public Task<PagedData<Order>> GetOrdersForSite(string site, int page, OrderFilter filter)
        {
            throw new NotImplementedException();
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
    }
}
