using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Models.Common;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IKenticoResourceService kenticoResources;

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

        public OrderService(IKenticoResourceService kenticoResources)
        {
            this.kenticoResources = kenticoResources;
        }

        public Task<FileResult> ExportOrders(DateTime? fromDate, DateTime? toDate, string format)
        {
            return Task.FromResult(new FileResult
            {
                Data = System.Text.UTF8Encoding.UTF8.GetBytes("test file"),
                Mime = "text/plain"
            });
        }

        public Task<TableView> GetOrdersView(DateTime? fromDate, DateTime? toDate, string sort, int page)
        {
            return Task.FromResult(new TableView());
        }
    }
}
