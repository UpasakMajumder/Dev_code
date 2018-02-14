using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IShoppingCartService
    {
        CheckoutPage GetCheckoutPage();
        CartItems GetCartItems();
        Task<CheckoutPageDeliveryTotals> GetDeliveryAndTotals();
        Task<CheckoutPageDeliveryTotals> SetDeliveryAddress(DeliveryAddress deliveryAddress);
        Task<CheckoutPageDeliveryTotals> SelectShipipng(int id);
        DeliveryAddresses SelectAddress(int id);
        CartItems ChangeItemQuantity(int id, int quantity);
        CartItems RemoveItem(int id);
        CartItemsPreview ItemsPreview();
        Task<AddToCartResult> AddToCart(NewCartItem item);
        List<int> GetLoggedInUserCartData(int inventoryType, int userID, int campaignID = 0);
    }
}
