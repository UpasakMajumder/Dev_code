using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Dto.ViewOrder.MicroserviceResponses;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena.KOrder.PaymentService.Infrastucture.Helpers;

namespace Kadena2.MicroserviceClients.Clients
{
    public class OrderViewClient : ClientBase, IOrderViewClient
    {
        //public OrderViewClient() : base()
        //{

        //}

        //public OrderViewClient(IAwsV4Signer signer) : base(signer)
        //{

        //}

        public async Task<BaseResponseDto<GetOrderByOrderIdResponseDTO>> GetOrderByOrderId(string serviceEndpoint, string orderId)
        {
            var url = $"{serviceEndpoint.TrimEnd('/')}/api/order/{orderId}";
            return await Get<GetOrderByOrderIdResponseDTO>(url);
        }

        public async Task<BaseResponseDto<OrderListDto>> GetOrders(string serviceEndpoint, int customerId, int pageNumber, int quantity)
        {
            var parameterizedUrl = $"{serviceEndpoint}?ClientId={customerId}&pageNumber={pageNumber}&quantity={quantity}";
            return await Get<OrderListDto>(parameterizedUrl);
        }

        public async Task<BaseResponseDto<OrderListDto>> GetOrders(string serviceEndpoint, string siteName, int pageNumber, int quantity)
        {
            var url = $"{serviceEndpoint}?siteName={siteName}&pageNumber={pageNumber}&quantity={quantity}";
            return await Get<OrderListDto>(url);
        }
    }
}

