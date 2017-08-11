using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.Product;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
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

        CartItem[] GetShoppingCartItems();

        void RemoveCartItem(int id);

        int GetShoppingCartItemsCount();

        void SetCartItemQuantity(int id, int quantity);

        void RemoveCurrentItemsFromStock();

        void RemoveCurrentItemsFromCart();

        double GetCurrentCartTotalItemsPrice();

        double GetCurrentCartShippingCost();

        IEnumerable<State> GetStates();

        void SaveShippingAddress(DeliveryAddress address);

        void SetCartItemDesignFilePath(int id, string path);

        string GetShippingProviderIcon(string title);

        string GetSkuImageUrl(int skuid);

        Product GetProductByDocumentId(int documentId);

        string GetDocumentUrl(int documentId);

        List<string> GetBreadcrumbs(int documentId);

        string GetProductTeaserImageUrl(int documentId);

        bool IsAuthorizedPerResource(string resourceName, string permissionName, string siteName);
		
        bool UserCanSeePrices();

        bool UserCanSeeAllOrders();

        Site GetSite(int siteId);

        CartItem AddCartItem(NewCartItem item, MailingList mailingList = null);
    }
}
