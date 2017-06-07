using Kadena.WebAPI.Models;

namespace Kadena.WebAPI.Contracts
{
    public interface IKenticoProviderService
    {
        DeliveryAddress[] GetCustomerAddresses();
        DeliveryAddress GetCurrentCartShippingAddress();
        BillingAddress GetDefaultBillingAddress();
        DeliveryCarrier[] GetShippingCarriers();
        DeliveryOption[] GetShippingOptions();
        ShoppingCartTotals GetShoppingCartTotals();
        PaymentMethod[] GetPaymentMethods();
        PaymentMethod GetPaymentMethod(int id);
        void SetShoppingCartAddres(int addressId);
        void SelectShipping(int shippingOptionsId);
        string GetResourceString(string name);

        int GetCurrentCartAddresId();
        int GetCurrentCartShippingOptionId();

        Customer GetCurrentCustomer();

        DeliveryOption GetShippingOption(int id);

        OrderItem[] GetShoppingCartOrderItems();

        CartItem[] GetShoppingCartItems();

        void RemoveCartItem(int id);
        void SetCartItemQuantity(int id, int quantity);
    }
}
