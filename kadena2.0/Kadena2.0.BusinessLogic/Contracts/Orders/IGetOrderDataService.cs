using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models.SubmitOrder;
using System.Threading.Tasks;

namespace Kadena2.BusinessLogic.Contracts.Orders
{
    public interface IGetOrderDataService
    {
        Task<OrderDTO> GetSubmitOrderData(SubmitOrderRequest request);
        AddressDTO GetSourceAddressForDeliveryEstimation();
    }
}
