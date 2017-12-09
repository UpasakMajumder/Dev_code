using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using System;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class POSController : ApiControllerBase
    {
        private readonly IPOSService posService;
        private readonly IMapper mapper;

        public POSController(IPOSService posService, IMapper mapper)
        {
            if (posService == null)
            {
                throw new ArgumentNullException(nameof(posService));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.mapper = mapper;
            this.posService = posService;
        }

        [HttpGet]
        [Route("api/pos/{posID}")]
        public IHttpActionResult TogglePOSStatus(int posID)
        {
            posService.TogglePOSStatus(posID);
            return ResponseJson<string>("OK");
        }
    }
}
