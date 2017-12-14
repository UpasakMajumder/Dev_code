using Kadena.Models;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoUserProvider
    {
        DeliveryAddress[] GetCustomerAddresses(AddressType addressType);
        DeliveryAddress[] GetCustomerAddresses(int customerId, AddressType addressType);
        Customer GetCurrentCustomer();
        Customer GetCustomer(int customerId);

        bool UserCanSeePrices();

        bool UserCanSeePrices(int siteId, int userId);
        bool UserCanSeeAllOrders();
        bool UserCanModifyShippingAddress();
        bool UserCanDownloadHiresPdf(int siteId, int userId);
        void SetDefaultShippingAddress(int addressId);
        User GetCurrentUser();
        User GetUser(string mail);
        bool SaveLocalization(string code);
        void UnsetDefaultShippingAddress();

        bool UserIsInCurrentSite(int userId);
    }
}
