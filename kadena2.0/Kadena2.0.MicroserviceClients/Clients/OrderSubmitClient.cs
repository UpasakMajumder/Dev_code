using Kadena.Dto.General;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.KOrder.PaymentService.Infrastucture.Helpers;

namespace Kadena2.MicroserviceClients.Clients
{
    public class OrderSubmitClient : ClientBase, IOrderSubmitClient
    {
        //public OrderSubmitClient(IAwsV4Signer signer) : base(signer)
        //{

        //}

        public async Task<BaseResponseDto<string>> FinishOrder(string serviceEndpoint, string orderNumber)
        {
            // TODO: Was timeout = 60s here, I think useless
            var url = $"{serviceEndpoint}/api/order";
            return await Patch<string>(url, orderNumber).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<string>> SubmitOrder(string serviceEndpoint, OrderDTO orderData)
        {
            // TODO: Was timeout = 60s here, I think useless
            var url = $"{serviceEndpoint}/api/order";
            return await Post<string>(url, orderData).ConfigureAwait(false);
        }
    }
}

