using Kadena.Models.Checkout;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IShoppingCartItemsProvider
    {
        int GetShoppingCartItemsCount();
        CheckoutCartItem[] GetCheckoutCartItems(bool showPrices = true);
        void RemoveCartItem(int id);
        void SetCartItemQuantity(CartItemEntity cartItemEntity, int quantity);
        void SaveCartItem(CartItemEntity item);
        void SetArtwork(CartItemEntity cartItem, int documentId);
        CartItemEntity GetOrCreateCartItem(NewCartItem newItem);
        CartItemEntity GetCartItemEntity(int cartItemId);
    }
}
