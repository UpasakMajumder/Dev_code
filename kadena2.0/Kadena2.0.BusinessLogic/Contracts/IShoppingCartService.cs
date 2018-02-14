using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.Product;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IShoppingCartService
    {
        Task<CheckoutPage> GetCheckoutPage();
        Task<CheckoutPageDeliveryTotals> GetDeliveryAndTotals();
        Task<CheckoutPageDeliveryTotals> SetDeliveryAddress(DeliveryAddress deliveryAddress);
        Task<CheckoutPage> SelectShipipng(int id);
        Task<CheckoutPage> SelectAddress(int id);
        Task<CheckoutPage> ChangeItemQuantity(int id, int quantity);
        Task<CheckoutPage> RemoveItem(int id);
        CartItemsPreview ItemsPreview();
        Task<AddToCartResult> AddToCart(NewCartItem item);
    }
}
