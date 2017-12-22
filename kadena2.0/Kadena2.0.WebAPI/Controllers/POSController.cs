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
        [Route("api/deletepos/{posID}")]
        public IHttpActionResult DeletePOS(int posID)
        {
            var isDeleted = posService.DeletePOS(posID);
            if (isDeleted)
            {
                return ResponseJson<string>("OK");
            }
            else
            {
                return ErrorJson("ERROR");
            }
        }
    }
}
