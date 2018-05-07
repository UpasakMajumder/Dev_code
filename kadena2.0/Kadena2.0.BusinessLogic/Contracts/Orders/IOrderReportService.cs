using Kadena.Models.Common;
using Kadena.Models.Orders;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts.Orders
{
    public interface IOrderReportService
    {
        int OrdersPerPage { get; set; }
        Task<PagedData<OrderReport>> GetOrders(int page, OrderFilter filter);
        Task<PagedData<OrderReport>> GetOrdersForSite(string site, int page, OrderFilter filter);

        TableView ConvertOrdersToView(PagedData<OrderReport> orders);

        Task<FileResult> GetOrdersExport(OrderFilter filter);
        Task<FileResult> GetOrdersExportForSite(string site, OrderFilter filter);
    }
}
