using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models.SubmitOrder;
using System.Threading.Tasks;

namespace Kadena2._0.BusinessLogic.Contracts.Orders
{
    public interface IGetOrderDataService
    {
        Task<OrderDTO> GetSubmitOrderData(SubmitOrderRequest request);
    }
}
