using Kadena.Models;
using Kadena.Models.Membership;

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
        void CreateUser(User user, int siteId, UserSettings userSettings = null);
        void UpdateUser(User user, UserSettings userSettings = null);
        void LinkCustomerToUser(int customerId, int userId);
        UserSettings GetUserSettings(int userId);
        void AcceptTaC();
    }
}
