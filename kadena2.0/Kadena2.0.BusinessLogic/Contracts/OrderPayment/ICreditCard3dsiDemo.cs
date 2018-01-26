using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models.SubmitOrder;

namespace Kadena2.BusinessLogic.Contracts.OrderPayment
{
    public interface ICreditCard3dsiDemo
    {
        SubmitOrderResult PayByCard3dsi(OrderDTO orderData);
    }
}
