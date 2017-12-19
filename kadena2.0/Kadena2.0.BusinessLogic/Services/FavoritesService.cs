using Kadena.Models.Product;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class FavoritesService : IFavoritesService
    {
        private readonly IKenticoFavoritesProvider kenticofavorites;

        public FavoritesService(IKenticoFavoritesProvider kenticofavorites)
        {
            if (kenticofavorites == null)
            {
                throw new ArgumentNullException(nameof(kenticofavorites));
            }

            this.kenticofavorites = kenticofavorites;
        }

        public void SetFavoriteProduct(int productDocumentId)
        {
            kenticofavorites.SetFavoriteProduct(productDocumentId);
        }

        public void UnsetFavoriteProduct(int productDocumentId)
        {
            kenticofavorites.UnsetFavoriteProduct(productDocumentId);
        }

        public List<int> CheckFavoriteProductIds(List<int> productIds)
        {
            return kenticofavorites.CheckFavoriteProductIds(productIds);
        }

        public List<ProductLink> GetFavorites(int count)
        {
            return kenticofavorites.GetFavorites(count);
        }
    }
}
