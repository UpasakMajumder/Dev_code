using Kadena.Models.Checkout;
using System;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Contracts
{
    public interface IShoppingCartService
    {
        CheckoutPage GetCheckoutPage();
        Task<CheckoutPageDeliveryTotals> GetDeliveryAndTotals();
        CheckoutPage SelectShipipng(int id);
        CheckoutPage SelectAddress(int id);
        CheckoutPage ChangeItemQuantity(int id, int quantity);
        CheckoutPage RemoveItem(int id);
        CartItemsPreview ItemsPreview();
        Task<CartItemsPreview> AddToCart(NewCartItem item);
    }
}
