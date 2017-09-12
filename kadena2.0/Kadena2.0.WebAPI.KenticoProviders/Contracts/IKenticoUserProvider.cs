using Kadena.Models;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoUserProvider
    {
        DeliveryAddress[] GetCustomerAddresses(string addressType = null);

        DeliveryAddress[] GetCustomerShippingAddresses(int customerId);
        
        Customer GetCurrentCustomer();

        Customer GetCustomer(int siteId, int customerId);
        bool UserCanSeePrices();
        bool UserCanSeePrices(int userId);

        bool UserCanSeeAllOrders();

        bool UserCanModifyShippingAddress();
    }
}
