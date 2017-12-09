using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using System;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class ProductCategoryController : ApiControllerBase
    {
        private readonly IProductCategoryService productCategoryService;
        private readonly IMapper mapper;

        public ProductCategoryController(IProductCategoryService productCategoryService, IMapper mapper)
        {
            if (productCategoryService == null)
            {
                throw new ArgumentNullException(nameof(productCategoryService));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.mapper = mapper;
            this.productCategoryService = productCategoryService;
        }

        [HttpDelete]
        [Route("api/deleteproductcategory/{categoryID}")]
        public IHttpActionResult DeleteCategory(int categoryID)
        {
            productCategoryService.DeleteCategory(categoryID);
            return ResponseJson<string>("OK");
        }
    }
}
