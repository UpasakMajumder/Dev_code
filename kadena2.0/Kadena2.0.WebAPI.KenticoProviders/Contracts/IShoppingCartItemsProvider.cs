using Kadena.Models.Checkout;
using Kadena.Models.Product;
using System;
using System.Collections.Generic;

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
        CartItemEntity GetOrCreateCartItem(Product product, int quantity, Dictionary<string, int> productAttributes, Guid templateId);
        CartItemEntity GetCartItemEntity(int cartItemId);
    }
}
