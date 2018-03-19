using Kadena.Models.Checkout;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IShoppingCartItemsProvider
    {
        int GetShoppingCartItemsCount();
        CartItem[] GetShoppingCartItems(bool showPrices = true);
        void RemoveCartItem(int id);
        void SetCartItemQuantity(int id, int quantity);
        void SaveCartItem(CartItemEntity item);
        void SetArtwork(CartItemEntity cartItem, int documentId);
        CartItemEntity GetOrCreateCartItem(NewCartItem newItem);
    }
}
