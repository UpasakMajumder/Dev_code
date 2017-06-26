using Kadena.Dto.General;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IOrderSubmitClient
    {
        Task<BaseResponseDto<string>> SubmitOrder(string serviceEndpoint, OrderDTO orderData);
    }
}
