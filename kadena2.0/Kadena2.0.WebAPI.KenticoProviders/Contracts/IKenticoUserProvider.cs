using Kadena.Models.Membership;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoUserProvider
    {                
        User GetCurrentUser();
        User GetUser(string mail);
        bool SaveLocalization(string code);        
        bool UserIsInCurrentSite(int userId);
        User GetUserByUserId(int userId);        
        void CreateUser(User user, int siteId, UserSettings userSettings = null);
        void UpdateUser(User user, UserSettings userSettings = null);
        void LinkCustomerToUser(int customerId, int userId);
        UserSettings GetUserSettings(int userId);
        void AcceptTaC();
    }
}
