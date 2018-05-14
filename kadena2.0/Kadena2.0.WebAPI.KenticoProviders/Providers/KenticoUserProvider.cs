using AutoMapper;
using CMS.Ecommerce;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Models.Membership;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Web;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoUserProvider : IKenticoUserProvider
    {
        private readonly IKenticoLogger _logger;
        private readonly IMapper _mapper;

        public KenticoUserProvider(IKenticoLogger logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public User GetUser(string mail)
        {
            return _mapper.Map<User>(UserInfoProvider.GetUserInfo(mail));
        }

        public User GetCurrentUser()
        {
            return _mapper.Map<User>(MembershipContext.AuthenticatedUser);
        }

        public bool SaveLocalization(string code)
        {
            try
            {
                var user = MembershipContext.AuthenticatedUser;
                if (user != null)
                {
                    user.PreferredCultureCode = code;
                    UserInfoProvider.SetUserInfo(user);
                }
            }
            catch (Exception exc)
            {
                _logger.LogException("UserProvider - Saving Localization", exc);
                throw;
            }
            return true;
        }

        public bool UserIsInCurrentSite(int userId)
        {
            var user = UserInfoProvider.GetUserInfo(userId);
            return user?.IsInSite(SiteContext.CurrentSiteName) ?? false;
        }

        public User GetUserByUserId(int userId)
        {
            return _mapper.Map<User>(UserInfoProvider.GetUserInfo(userId));
        }
       
        public void CreateUser(User user, int siteId)
        {
            var newUser = _mapper.Map<UserInfo>(user);
            newUser.Enabled = true;
            newUser.UserSettings.UserRegistrationInfo.IPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            newUser.UserSettings.UserRegistrationInfo.Agent = HttpContext.Current.Request.UserAgent;
            newUser.Insert();
            
            var newUserId = newUser.UserID;
            UserSiteInfoProvider.AddUserToSite(newUserId, siteId);
            user.UserId = newUserId;
        }

        public void UpdateUser(User user)
        {
            var userInfo = UserInfoProvider.GetUserInfo(user.UserId);

            if (userInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(user.UserId), "Existing User with given Id not found");
            }

            userInfo.UserName = user.UserName;
            userInfo.FirstName = user.FirstName;
            userInfo.LastName = user.LastName;
            userInfo.Email = user.Email;
            userInfo.UserURLReferrer = user.CallBackUrl;

            userInfo.Update();
        }
        
        public void LinkCustomerToUser(int customerId, int userId)
        {
            var customer = CustomerInfoProvider.GetCustomerInfo(customerId);
            customer.CustomerUserID = userId;
            customer.Update();
        }        

        public void AcceptTaC()
        {
            var user = MembershipContext.AuthenticatedUser;
            user.SetValue("TermsConditionsAccepted", DateTime.Now);
            user.Update();
        }

        public void SetPassword(int userId, string password)
        {
            var userInfo = UserInfoProvider.GetUserInfo(userId);
            UserInfoProvider.SetPassword(userInfo, password);
        }
    }
}