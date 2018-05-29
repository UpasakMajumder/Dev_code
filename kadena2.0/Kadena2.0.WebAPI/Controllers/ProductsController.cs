using System.Web.Http;
using System;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.Product.Responses;
using System.Collections.Generic;
using Kadena.Dto.Product;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class ProductsController : ApiControllerBase
    {
        private readonly IProductsService products;
        private readonly IMapper mapper;

        public ProductsController(IProductsService products, IMapper mapper)
        {
            this.products = products ?? throw new ArgumentNullException(nameof(products));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        [HttpGet]
        [Route("api/products")]
        [QuerystringParameterRequired("url")]
        public IHttpActionResult GetProducts([FromUri]string url)
        {
            var result = products.GetProducts(url);
            var resultDto = mapper.Map<GetProductsDto>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/products/getprice/{skuId:int}")]
        public IHttpActionResult GetPrice([FromUri]int skuId, [FromBody]Dictionary<string, int> options)
        {
            var price = products.GetPrice(skuId, options);
            var result = mapper.Map<PriceDto>(price);
            return ResponseJson(result);
        }

        [HttpGet]
        [Route("api/products/availability/{skuId:int}")]
        public IHttpActionResult GetAvailability([FromUri]int skuid)
        {
            var result = products.GetInventoryProductAvailability(skuid);
            var resultDto = mapper.Map<ProductAvailabilityDto>(result);
            return ResponseJson(resultDto);
        }
    }
}
