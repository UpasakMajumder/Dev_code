using System.Collections.Generic;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Dto.Routing;
using Kadena.Dto.Routing.MicroserviceRequests;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IRoutingServiceClient
    {
        Task<BaseResponseDto<IEnumerable<RoutingDto>>> GetRoutings(string orderId = null,
            string siteId = null);

        Task<BaseResponseDto<RoutingDto>> SetRouting(SetRoutingRequestDto requestBody);

        Task<BaseResponseDto<object>> DeleteRouting(DeleteRoutingRequestDto requestBody);
    }
}
