﻿using Kadena.WebAPI.Contracts;
using System.Web.Http;
using System;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using System.Collections.Generic;
using System.Linq;
using Kadena.Dto.Favorites.Requests;

namespace Kadena.WebAPI.Controllers
{
    public class FavoritesController : ApiControllerBase
    {
        private readonly IFavoritesService favorites;
        
        public FavoritesController(IFavoritesService favorites)
        {
            if (favorites == null)
            {
                throw new ArgumentNullException(nameof(favorites));
            }

            this.favorites = favorites;
        }

        [HttpPost]
        [Route("api/favorites/set/{productDocumentId}")]
        [AuthorizationFilter]
        public IHttpActionResult SetFavorite(int productDocumentId)
        {
            favorites.SetFavoriteProduct(productDocumentId);
            return ResponseJson<string>("OK");
        }

        [HttpPost]
        [Route("api/favorites/unset/{productDocumentId}")]
        [AuthorizationFilter]
        public IHttpActionResult UnsetFavorite(int productDocumentId)
        {
            favorites.UnsetFavoriteProduct(productDocumentId);
            return ResponseJson<string>("OK");
        }

        [HttpPost]
        [Route("api/favorites/check")]
        [AuthorizationFilter]
        public IHttpActionResult CheckFavorite([FromBody] CheckFavoritesRequestDto par)
        {
            var ids = favorites.CheckFavoriteProductIds(par.ProductIds);
            return ResponseJson<List<int>>(ids);
        }
    }    
}
