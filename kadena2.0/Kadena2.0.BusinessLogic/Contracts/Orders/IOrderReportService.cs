using Kadena.Models.Common;
using Kadena.Models.Orders;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts.Orders
{
    public interface IOrderReportService
    {
        int OrdersPerPage { get; set; }

        Task<TableView> ConvertOrdersToView(int page, OrderFilter filter);

        Task<FileResult> GetOrdersExport(OrderFilter filter);

        Task<FileResult> GetOrdersExportForSite(string site, OrderFilter filter);

        Task<PagedData<OrderReportViewItem>> GetOrdersForSite(string site, int page, OrderFilter filter);
    }
}
