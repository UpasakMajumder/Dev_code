﻿using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.Product;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoProviderService
    {
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

        DeliveryOption GetShippingOption(int id);

        CartItem[] GetShoppingCartItems(bool showPrices = true);

        void RemoveCartItem(int id);

        int GetShoppingCartItemsCount();

        void SetCartItemQuantity(int id, int quantity);

        void RemoveCurrentItemsFromStock();

        void RemoveCurrentItemsFromCart();

        double GetCurrentCartTotalItemsPrice();

        double GetCurrentCartShippingCost();

        IEnumerable<State> GetStates();

        void SaveShippingAddress(DeliveryAddress address);

        string GetShippingProviderIcon(string title);

        string GetSkuImageUrl(int skuid);

        Product GetProductByDocumentId(int documentId);

        string GetDocumentUrl(int documentId);

        List<string> GetBreadcrumbs(int documentId);

        string GetProductTeaserImageUrl(int documentId);

        bool IsAuthorizedPerResource(string resourceName, string permissionName, string siteName);

        Site[] GetSites();

        CartItem AddCartItem(NewCartItem item, MailingList mailingList = null);

        string MapOrderStatus(string microserviceStatus);

        void SetSkuAvailableQty(string skunumber, int availableItems);
    }
}
