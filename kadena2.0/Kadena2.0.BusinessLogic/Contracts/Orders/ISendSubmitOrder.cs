using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models.SubmitOrder;
using System.Threading.Tasks;

namespace Kadena2._0.BusinessLogic.Contracts.Orders
{
    public interface ISendSubmitOrder
    {
        Task<SubmitOrderResult> SubmitOrderData(OrderDTO orderData);
    }
}
