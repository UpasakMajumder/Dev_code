using Kadena.Models.Common;
using System;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts.Orders
{
    public interface IOrderService
    {
        Task<TableView> GetOrdersView(DateTime? fromDate, DateTime? toDate, string sort, int page);
        Task<FileResult> ExportOrders(DateTime? fromDate, DateTime? toDate, string format);
    }
}
