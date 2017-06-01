using Kadena.WebAPI.Models;

namespace Kadena.WebAPI.Contracts
{
    public interface ICMSProviderService
    {
        DeliveryAddress[] GetCustomerAddresses();
        DeliveryMethod[] GetShippingCarriers();
        DeliveryService[] GetShippingOptions();
        Total[] GetShoppingCartTotals();
        PaymentMethod[] GetPaymentMethods();
        void SetShoppingCartAddres(int addressId);
        void SelectShipping(int shippingOptionsId);
        string GetResourceString(string name);

        int GetCurrentCartAddresId();
        int GetCurrentCartShippingMethodId();
    }
}
