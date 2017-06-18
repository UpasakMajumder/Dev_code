﻿using Kadena.WebAPI.Models;
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
        int GetCurrentCartAddresId();

        int GetCurrentCartShippingOptionId();
        Customer GetCurrentCustomer();
        Customer GetCustomer(int customerId);
        DeliveryOption GetShippingOption(int id);
        CartItem[] GetShoppingCartOrderItems();
        CartItem[] GetShoppingCartItems();
        void RemoveCartItem(int id);

        void SetCartItemQuantity(int id, int quantity);
        void RemoveCurrentItemsFromStock();

        void RemoveCurrentItemsFromCart();
        double GetCurrentCartTotalItemsPrice();
        double GetCurrentCartShippingCost();

        IEnumerable<State> GetStates();

        void SaveShippingAddress(DeliveryAddress address);

        void SetCartItemDesignFilePath(int id, string path);
    }
}
