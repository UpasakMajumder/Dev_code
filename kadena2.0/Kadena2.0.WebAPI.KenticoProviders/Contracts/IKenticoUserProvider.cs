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
        void CreateUser(User user, int siteId);
        void UpdateUser(User user);
        void LinkCustomerToUser(int customerId, int userId);
        void AcceptTaC();
        void SetPassword(int userId, string password);
    }
}
