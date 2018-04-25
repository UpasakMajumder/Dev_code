using Kadena.Models;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
   public interface IKenticoCustomerProvider
    {
        Customer GetCurrentCustomer();
        Customer GetCustomer(int customerId);
        Customer GetCustomerByUser(int userId);
        int CreateCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        int GetUserIDByCustomerID(int customerID);
        int GetCustomerIDByUserID(int userID);
    }
}
