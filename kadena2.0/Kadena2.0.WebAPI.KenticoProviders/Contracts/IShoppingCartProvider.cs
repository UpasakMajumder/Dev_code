using CMS.Ecommerce;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.CustomerData;
using System;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IShoppingCartProvider
    {
        DeliveryAddress GetCurrentCartShippingAddress();

        BillingAddress GetDefaultBillingAddress();

        DeliveryCarrier[] GetShippingCarriers();

        DeliveryOption[] GetShippingOptions();

        ShoppingCartTotals GetShoppingCartTotals();

        PaymentMethod[] GetPaymentMethods();

        PaymentMethod GetPaymentMethod(int id);

        void SetShoppingCartAddress(int addressId);

        void SetShoppingCartAddress(DeliveryAddress address);

        void SelectShipping(int shippingOptionsId);

        int GetCurrentCartAddresId();

        int GetCurrentCartShippingOptionId();

        DeliveryOption GetShippingOption(int id);

        CartItem[] GetShoppingCartItems(bool showPrices = true);

        void RemoveCartItem(int id);

        int GetShoppingCartItemsCount();

        void SetCartItemQuantity(int id, int quantity);

        void RemoveCurrentItemsFromStock();

        void ClearCart();

        double GetCurrentCartTotalItemsPrice();

        double GetCurrentCartShippingCost();

        void SaveShippingAddress(DeliveryAddress address);

        string GetShippingProviderIcon(string title);

        CartItem AddCartItem(NewCartItem item, MailingList mailingList = null);

        Tuple<string, bool> UpdateCartQuantity(Distributor data);
        List<int> GetUserIDsWithShoppingCart(int campaignID, int productType);
    }
}
