using Kadena.Models;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoUserProvider
    {        
        Customer GetCurrentCustomer();
        Customer GetCustomer(int customerId);                
        Customer GetCustomerByUser(int userId);                
        User GetCurrentUser();
        User GetUser(string mail);
        bool SaveLocalization(string code);        
        bool UserIsInCurrentSite(int userId);
        User GetUserByUserId(int userId);
        int CreateCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void CreateUser(User user, int siteId);
        void UpdateUser(User user);
        void LinkCustomerToUser(int customerId, int userId);
        void AcceptTaC(string mail);
    }
}
