using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.Routing.MicroserviceRequests;
using Kadena.Models.Routing;
using Kadena.Models.Routing.Request;
using Kadena2.MicroserviceClients.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class RoutingService : IRoutingService
    {
        private readonly IRoutingServiceClient _client;
        private readonly IMapper _mapper;

        public RoutingService(IRoutingServiceClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Routing>> GetRoutings(string orderId = null, string siteId = null)
        {
            var response = await _client.GetRoutings(orderId, siteId);

            return _mapper.Map<Routing[]>(response.Payload);
        }

        public async Task<Routing> SetRouting(SetRouting data)
        {
            var request = _mapper.Map<SetRoutingRequestDto>(data);
            var response = await _client.SetRouting(request);

            return _mapper.Map<Routing>(response.Payload);
        }

        public async Task<bool> DeleteRouting(DeleteRouting data)
        {
            var request = _mapper.Map<DeleteRoutingRequestDto>(data);
            var response = await _client.DeleteRouting(request);

            return response.Success;
        }
    }
}
