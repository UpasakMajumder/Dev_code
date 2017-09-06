using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.WebAPI.Services
{
    public class FavoritesService : IFavoritesService
    {
        private readonly IKenticoFavoritesProvider kenticofavorites;

        public FavoritesService(IKenticoFavoritesProvider kenticofavorites)
        {
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
    }
}
