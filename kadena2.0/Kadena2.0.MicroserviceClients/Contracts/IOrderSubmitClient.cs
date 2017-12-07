using Kadena.Dto.General;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IOrderSubmitClient
    {
        /// <summary>
        /// Creates order.
        /// </summary>
        /// <param name="serviceEndpoint"></param>
        /// <param name="orderData"></param>
        /// <returns>Order number</returns>
        Task<BaseResponseDto<string>> SubmitOrder(OrderDTO orderData);
    }
}
