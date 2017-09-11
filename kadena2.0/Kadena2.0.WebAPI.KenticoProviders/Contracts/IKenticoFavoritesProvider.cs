using System.Collections.Generic;
using System.Data;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoFavoritesProvider
    {
        void SetFavoriteProduct(int productDocumentId);
        void UnsetFavoriteProduct(int productDocumentId);        
        List<int> CheckFavoriteProductIds(List<int> productIds);
    }
}