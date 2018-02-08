using Kadena.Models.OrderDetail;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IOrderDetailService
    {
        Task<OrderDetail> GetOrderDetail(string orderId);
    }
}
