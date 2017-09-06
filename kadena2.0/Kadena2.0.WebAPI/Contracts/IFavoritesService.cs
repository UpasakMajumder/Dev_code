namespace Kadena.WebAPI.Contracts
{
    public interface IFavoritesService
    {
        void SetFavoriteProduct(int productDocumentId);
        void UnsetFavoriteProduct(int productDocumentId);
    }
}