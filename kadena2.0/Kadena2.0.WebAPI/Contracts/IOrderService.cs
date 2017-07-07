using Kadena.WebAPI.Models.OrderDetail;
using Kadena.WebAPI.Models.SubmitOrder;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Contracts
{
    public interface IOrderService
    {
        Task<SubmitOrderResult> SubmitOrder(SubmitOrderRequest request);
			
        Task<OrderDetail> GetOrderDetail(string orderId);
    }
}
