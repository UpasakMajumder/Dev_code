using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.Brands;
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

        [HttpGet]
        [Route("api/getbrands")]
        public IHttpActionResult GetBrands()
        {
            var brands = brandsService.GetBrands();
            var brandsDto = mapper.Map<BrandDto[]>(brands);
            return ResponseJson(brands);
        }

        [HttpPost]
        [Route("api/getaddressbrands/{addressID}")]
        public IHttpActionResult GetAddressBrands(int addressID)
        {
            var addressBrands = brandsService.GetAddressBrands(addressID);
            var brandsDto = mapper.Map<BrandDto[]>(addressBrands);
            return ResponseJson(addressBrands);
        }
    }
}