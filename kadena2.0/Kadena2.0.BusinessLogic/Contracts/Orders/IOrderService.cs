using Kadena.Models.Common;
using Kadena.Models.Orders;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts.Orders
{
    public interface IOrderReportService
    {
        Task<PagedData<OrderReport>> GetOrders(int page, OrderFilter filter);
        Task<PagedData<OrderReport>> GetOrdersForSite(string site, int page, OrderFilter filter);

        TableView ConvertOrdersToView(PagedData<OrderReport> orders);

        Task<FileResult> GetOrdersExport(string format, OrderFilter filter);
        Task<FileResult> GetOrdersExportForSite(string site, string format, OrderFilter filter);
    }
}
