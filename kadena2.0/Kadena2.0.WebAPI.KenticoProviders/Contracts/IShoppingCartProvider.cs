using CMS.DataEngine;
using CMS.Ecommerce;
using Kadena.Models;
using Kadena.Models.AddToCart;
using Kadena.Models.Checkout;
using Kadena.Models.CustomerData;
using Kadena.Models.Product;
using Kadena.Models.ShoppingCarts;
using System;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IShoppingCartProvider
    {
        DeliveryAddress GetCurrentCartShippingAddress();

        DeliveryAddress GetAddress(int addressId);

        BillingAddress GetDefaultBillingAddress();

        DeliveryCarrier[] GetShippingCarriers();

        DeliveryOption[] GetShippingOptions();

        ShoppingCartTotals GetShoppingCartTotals();

        PaymentMethod[] GetPaymentMethods();

        PaymentMethod GetPaymentMethod(int id);
        DateTime? GetRequestedDeliveryDate();
        void SetRequestedDeliveryDate(DateTime? requestedDeliveryDate);

        void SetShoppingCartAddress(int addressId);

        void SelectShipping(int shippingOptionsId);

        int GetCurrentCartShippingOptionId();

        DeliveryOption GetShippingOption(int id);

        DeliveryOption GetShippingOption(string name);

        int GetShoppingCartId(int userId, int siteId);

        void RemoveCurrentItemsFromStock(int shoppingCartId = 0);

        void ClearCart(int shoppingCartId = 0);

        double GetCurrentCartTotalItemsPrice();

        double GetCurrentCartShippingCost();

        string GetShippingProviderIcon(string title);

        string UpdateCartQuantity(Distributor data);

        List<int> GetUserIDsWithShoppingCart(int campaignID, int productType);

        void UpdateBusinessUnit(ShoppingCartInfo cart, long businessUnitID);

        bool IsCartContainsInvalidProduct(int shoppingCartId = 0);

        List<int> GetCampaingShoppingCartIDs(int campaignID);

        List<int> GetUserShoppingCartIDs(int userID);

        bool ValidateAllCarts(int userID = 0, int campaignID = 0);

        List<int> GetShoppingCartIDByInventoryType(CampaignProductType cartType, int userID, int campaignID = 0);

        int GetPreBuyDemandCount(int SKUID);

        int GetDistributorCartID(int distributorID, CampaignProductType cartType = CampaignProductType.GeneralInventory, int campaignID = 0);

        int GetItemQuantity(int SKUID, int shoppingCartID);

        void UpdateDistributorCart(DistributorCartItem distributorCartItem, CampaignsProduct product, CampaignProductType cartType = CampaignProductType.GeneralInventory);

        void AddDistributorCartItem(int cartID, DistributorCartItem distributorCartItem, CampaignsProduct product, CampaignProductType cartType = CampaignProductType.GeneralInventory);

        void DeleteDistributorCartItem(int cartID, int SKUID);

        int GetDistributorCartCount(int userID, int campaignID, CampaignProductType cartType = CampaignProductType.GeneralInventory);

        decimal GetCartWeight(int cartId);

        void DeleteShoppingCart(int cartId);

        ShoppingCart GetShoppingCart(int cartId);

        int SaveCart(ShoppingCart cart);
    }
}
