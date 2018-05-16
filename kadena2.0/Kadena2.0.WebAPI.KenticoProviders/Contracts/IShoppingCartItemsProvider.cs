using Kadena.Models.Checkout;
using Kadena.Models.SubmitOrder;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IShoppingCartItemsProvider
    {
        int GetShoppingCartItemsCount();
        CheckoutCartItem[] GetCheckoutCartItems(bool showPrices = true);
        OrderCartItem[] GetOrderCartItems();
        void RemoveCartItem(int id);
        void SetCartItemQuantity(int id, int quantity);
        void SaveCartItem(CartItemEntity item);
        CartItemEntity GetOrCreateCartItem(NewCartItem newItem);
    }
}
