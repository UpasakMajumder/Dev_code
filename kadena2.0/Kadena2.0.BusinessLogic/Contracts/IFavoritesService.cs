using Kadena.Models.Product;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IFavoritesService
    {
        void SetFavoriteProduct(int productDocumentId);
        void UnsetFavoriteProduct(int productDocumentId);
        List<int> CheckFavoriteProductIds(List<int> productIds);
        List<ProductLink> GetFavorites(int count);
    }
}