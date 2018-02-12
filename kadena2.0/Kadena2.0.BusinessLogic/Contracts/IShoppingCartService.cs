using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.Product;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IShoppingCartService
    {
        CheckoutPage GetCheckoutPage();
        Task<CheckoutPageDeliveryTotals> GetDeliveryAndTotals();
        Task<CheckoutPageDeliveryTotals> SetDeliveryAddress(DeliveryAddress deliveryAddress);
        void SelectShipipng(int id);
        CheckoutPage SelectAddress(int id);
        CartItems ChangeItemQuantity(int id, int quantity);
        CartItems RemoveItem(int id);
        CartItemsPreview ItemsPreview();
        Task<AddToCartResult> AddToCart(NewCartItem item);
    }
}
