using Kadena.WebAPI.Contracts;
using System.Web.Http;
using System;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using Kadena.Dto.CustomerData;
using Kadena.Dto.Checkout.Requests;

namespace Kadena.WebAPI.Controllers
{
    public class CustomerDataController : ApiControllerBase
    {
        private readonly ICustomerDataService service;
        private readonly IMapper mapper;

        public CustomerDataController(ICustomerDataService service, IMapper mapper)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.service = service;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("api/customerdata")]
        public IHttpActionResult CustomerData([FromBody]CustomerDataRequestDto request)
        {
            var result = service.GetCustomerData(request.CustomerId);

            if (result == null)
                return ErrorJson($"Failed to retrieve customer data for customerId: {request.CustomerId}");

            var resultDto = mapper.Map<CustomerDataDTO>(result);
            return ResponseJson(resultDto);
        }
    }
}
