using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using System;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class BrandsController : ApiControllerBase
    {
        private readonly IBrandsService brandsService;
        private readonly IMapper mapper;

        public BrandsController(IBrandsService brandsService, IMapper mapper)
        {
            if (brandsService == null)
            {
                throw new ArgumentNullException(nameof(brandsService));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.mapper = mapper;
            this.brandsService = brandsService;
        }

        [HttpDelete]
        [Route("api/deletebrand/{brandID}")]
        public IHttpActionResult TogglePOSStatus(int brandID)
        {
            brandsService.DeleteBrand(brandID);
            return ResponseJson<string>("OK");
        }
    }
}
