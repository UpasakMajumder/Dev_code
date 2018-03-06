using CMS.DataEngine;
using CMS.Ecommerce;
using Kadena.Models;
using Kadena.Models.AddToCart;
using Kadena.Models.Checkout;
using Kadena.Models.CustomerData;
using Kadena.Models.Product;
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

        void SetShoppingCartAddress(int addressId);

        void SetShoppingCartAddress(DeliveryAddress address);

        void SelectShipping(int shippingOptionsId);

        int GetCurrentCartShippingOptionId();

        DeliveryOption GetShippingOption(int id);

        CartItem[] GetShoppingCartItems(bool showPrices = true);

        void RemoveCartItem(int id);

        int GetShoppingCartItemsCount();

        void SetCartItemQuantity(int id, int quantity);

        int GetShoppingCartId(int userId, int siteId);

        void RemoveCurrentItemsFromStock(int shoppingCartId = 0);

        void ClearCart(int shoppingCartId = 0);

        double GetCurrentCartTotalItemsPrice();

        double GetCurrentCartShippingCost();

        void SaveShippingAddress(DeliveryAddress address);

        string GetShippingProviderIcon(string title);

        CartItem AddCartItem(NewCartItem item, MailingList mailingList = null);

        string UpdateCartQuantity(Distributor data);

        List<int> GetUserIDsWithShoppingCart(int campaignID, int productType);

        ShoppingCartInfo GetShoppingCartByID(int cartID);

        List<int> GetShoppingCartIDs(WhereCondition where);

        List<ShoppingCartItemInfo> GetShoppingCartItemsByCartIDs(List<int> cartIDs);

        void UpdateBusinessUnit(ShoppingCartInfo cart, long businessUnitID);

        bool IsCartContainsInvalidProduct(int shoppingCartId = 0);

        List<int> GetCampaingShoppingCartIDs(int campaignID);

        List<int> GetUserShoppingCartIDs(int userID);

        bool ValidateAllCarts(int userID = 0, int campaignID = 0);

        List<int> GetShoppingCartIDByInventoryType(int inventoryType, int userID, int campaignID = 0);

        int GetPreBuyDemandCount(int SKUID);

        int GetDistributorCartID(int distributorID, int inventoryType = 1);

        int GetAllocatedQuantity(int SKUID, int userID);

        int GetItemQuantity(int SKUID, int shoppingCartID);

        int CreateDistributorCart(DistributorCartItem distributorCartItem, CampaignsProduct product, int userID, int inventoryType = 1);

        void UpdateDistributorCart(DistributorCartItem distributorCartItem, CampaignsProduct product, int inventoryType = 1);

        void AddDistributorCartItem(int cartID, DistributorCartItem distributorCartItem, CampaignsProduct product, int inventoryType = 1);

        void DeleteDistributorCartItem(int cartID, int SKUID);

        int GetDistributorCartCount(int userID, int campaignID, int inventoryType = 1);
    }
}
