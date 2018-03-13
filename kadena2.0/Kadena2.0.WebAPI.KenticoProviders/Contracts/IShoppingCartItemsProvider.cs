using Kadena.Models.Checkout;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IShoppingCartItemsProvider
    {
        int GetShoppingCartItemsCount();
        CartItem[] GetShoppingCartItems(bool showPrices = true);
        void RemoveCartItem(int id);
        void SetCartItemQuantity(int id, int quantity);
        void SaveCartItem(CartItem item);
        void SetArtwork(CartItem cartItem);
        CartItem EnsureCartItem(NewCartItem newItem);
    }
}
