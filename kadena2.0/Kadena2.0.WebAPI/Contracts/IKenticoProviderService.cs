using Kadena.WebAPI.Models;
using System.Collections.Generic;

namespace Kadena.WebAPI.Contracts
{
    public interface IKenticoProviderService
    {
        DeliveryAddress[] GetCustomerAddresses(string addressType = null);

        DeliveryAddress[] GetCustomerShippingAddresses(int customerId);
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

        Customer GetCustomer(int customerId);

        DeliveryOption GetShippingOption(int id);

        OrderItem[] GetShoppingCartOrderItems();

        CartItem[] GetShoppingCartItems();

        void RemoveCartItem(int id);

        void SetCartItemQuantity(int id, int quantity);

        int GetProductStockQuantity(int productId);

        void SetProductStockQuantity(int productId, int quantity);

        void RemoveCurrentItemsFromStock();

        void RemoveCurrentItemsFromCart();

        IEnumerable<State> GetStates();

        void SaveShippingAddress(DeliveryAddress address);
    }
}
