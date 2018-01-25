using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models.SubmitOrder;
using System.Threading.Tasks;

namespace Kadena2.BusinessLogic.Contracts.OrderPayment
{
    public interface ICreditCard3dsiDemo
    {
        Task<SubmitOrderResult> PayByCard3dsi(OrderDTO orderData);
    }
}
