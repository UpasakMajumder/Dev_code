using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models.SubmitOrder;
using System.Threading.Tasks;

namespace Kadena2.BusinessLogic.Contracts.OrderPayment
{
    public interface IPurchaseOrder
    {
        Task<SubmitOrderResult> SubmitPOOrder(OrderDTO orderData);
    }
}
