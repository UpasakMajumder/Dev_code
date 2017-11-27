using Kadena.BusinessLogic.Contracts;
using System.Web.Http;
using System;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using Kadena.Dto.CustomerData;
using Kadena.WebAPI.Infrastructure.Filters.Authentication;

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

        [HttpGet]
        [Route("api/customer/{customerId}/site/{siteId}")]
        [IdentityBasicAuthentication]
        public IHttpActionResult CustomerData(int siteId, int customerId)
        {
            var result = service.GetCustomerData(siteId, customerId);
            var resultDto = mapper.Map<CustomerDataDTO>(result);
            return ResponseJsonCheckingNull(resultDto, $"Failed to retrieve customer data for customerId: {customerId}"); 
        }
    }
}
