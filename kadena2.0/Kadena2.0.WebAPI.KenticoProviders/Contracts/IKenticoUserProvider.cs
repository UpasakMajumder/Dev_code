using Kadena.Models;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoUserProvider
    {
        DeliveryAddress[] GetCustomerAddresses(AddressType addressType);
        DeliveryAddress[] GetCustomerAddresses(int customerId, AddressType addressType);
        Customer GetCurrentCustomer();
        Customer GetCustomer(int customerId);        
        void SetDefaultShippingAddress(int addressId);
        User GetCurrentUser();
        User GetUser(string mail);
        bool SaveLocalization(string code);
        void UnsetDefaultShippingAddress();
        bool UserIsInCurrentSite(int userId);
        User GetUserByUserId(int userId);
        int CreateCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        int CreateUser(User user);
        void UpdateUser(User user);
        void LinkCustomerToUser(int customerId, int userId);
    }
}
