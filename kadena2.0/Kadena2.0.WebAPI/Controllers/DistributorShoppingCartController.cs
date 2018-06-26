using Kadena.BusinessLogic.Contracts;
using System.Web.Http;
using System;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using Kadena.Dto.AddToCart;
using Kadena.Models.AddToCart;
using Kadena.Dto.CustomerData;
using Kadena.Models.CustomerData;

namespace Kadena.WebAPI.Controllers
{
    public class DistributorShoppingCartController : ApiControllerBase
    {
        private readonly IDistributorShoppingCartService service;
        private readonly IMapper mapper;

        public DistributorShoppingCartController(
            IDistributorShoppingCartService service,
            IMapper mapper)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Route("api/getcartdistributordata/{skuID}/{inventoryType}")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult GetCartDistributorData(int skuID, int inventoryType)
        {
            var distributorData = service.GetCartDistributorData(skuID, inventoryType);
            var result = mapper.Map<DistributorCartDto>(distributorData);
            return ResponseJson(result);
        }

        [HttpPost]
        [Route("api/updatedistributorcarts")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult UpdateDistributorCarts([FromBody]DistributorCartDto request)
        {
            var submitRequest = mapper.Map<DistributorCart>(request);
            var serviceResponse = service.UpdateDistributorCarts(submitRequest);
            return ResponseJson(new { cartCount = serviceResponse });
        }

        [HttpPost]
        [Route("api/distributor/update")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult UpdateData([FromBody]DistributorDTO request)
        {
            var submitRequest = mapper.Map<Distributor>(request);
            var serviceResponse = service.UpdateCartQuantity(submitRequest);
            return ResponseJson<string>(serviceResponse);
        }
    }
}
