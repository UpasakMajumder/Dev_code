using Kadena.WebAPI.Models;

namespace Kadena.WebAPI.Contracts
{
    public interface ICMSProviderService
    {
        DeliveryAddress[] GetCustomerAddresses();
        DeliveryAddress GetCurrentCartShippingAddress();
        BillingAddress GetDefaultBillingAddress();
        DeliveryMethod[] GetShippingCarriers();
        DeliveryService[] GetShippingOptions();
        ShoppingCartTotals GetShoppingCartTotals();
        PaymentMethod[] GetPaymentMethods();
        PaymentMethod GetPaymentMethod(int id);
        void SetShoppingCartAddres(int addressId);
        void SelectShipping(int shippingOptionsId);
        string GetResourceString(string name);

        int GetCurrentCartAddresId();
        int GetCurrentCartShippingMethodId();

        Customer GetCurrentCustomer();

        DeliveryService GetShippingOption(int id);

        ShoppingCartItem[] GetShoppingCartItems();
    }
}
