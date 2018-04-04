using Kadena.Models.Common;
using Kadena.Models.Orders.Failed;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts.Orders
{
    public interface IOrderResubmissionService
    {
        Task<OperationResult> ResubmitOrder(string orderId);
        Task<PagedData<FailedOrder>> GetFailedOrders(int page, int itemsPerPage);
    }
}
