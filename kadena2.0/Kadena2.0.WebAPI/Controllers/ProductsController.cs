using System.Web.Http;
using System;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.Product.Responses;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class ProductsController : ApiControllerBase
    {
        private readonly IProductsService products;
        private readonly IMapper mapper;

        public ProductsController(IProductsService products, IMapper mapper)
        {
            if (products == null)
            {
                throw new ArgumentNullException(nameof(products));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.products = products;
            this.mapper = mapper;
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
    }
}
