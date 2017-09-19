using System.Collections.Generic;
using System.Data;
using Kadena.Models.Product;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoFavoritesProvider
    {
        void SetFavoriteProduct(int productDocumentId);
        void UnsetFavoriteProduct(int productDocumentId);        
        List<int> CheckFavoriteProductIds(List<int> productIds);
        List<ProductLink> GetFavorites(int count);
    }
}