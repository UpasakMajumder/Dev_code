using Kadena.BusinessLogic.Contracts;
using System.Web.Http;
using System;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using Kadena.Dto.Product;
using AutoMapper;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class FavoritesController : ApiControllerBase
    {
        private readonly IFavoritesService favorites;
        private readonly IMapper mapper;

        public FavoritesController(IFavoritesService favorites, IMapper mapper)
        {
            if (favorites == null)
            {
                throw new ArgumentNullException(nameof(favorites));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.mapper = mapper;
            this.favorites = favorites;
        }

        [HttpPut]
        [Route("api/favorites/set/{productDocumentId}")]
        public IHttpActionResult SetFavorite(int productDocumentId)
        {
            favorites.SetFavoriteProduct(productDocumentId);
            return ResponseJson<string>("OK");
        }

        [HttpPut]
        [Route("api/favorites/unset/{productDocumentId}")]
        public IHttpActionResult UnsetFavorite(int productDocumentId)
        {
            favorites.UnsetFavoriteProduct(productDocumentId);
            return ResponseJson<string>("OK");
        }

        [HttpGet]
        [Route("api/favorites/{count}")]
        public IHttpActionResult GetFavorites(int count = 5)
        {
            var products = favorites.GetFavorites(count);
            var productsDto = mapper.Map<ProductDto[]>(products);
            return ResponseJson<ProductDto[]>(productsDto);
        }
    }    
}
