using AutoMapper;
using CMS.Ecommerce;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoUserProvider : IKenticoUserProvider
    {
        private readonly IKenticoLogger _logger;
        private readonly IMapper _mapper;

        public KenticoUserProvider(IKenticoLogger logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }        

        public Customer GetCurrentCustomer()
        {
            return _mapper.Map<Customer>(ECommerceContext.CurrentCustomer);
        }

        public Customer GetCustomer(int customerId)
        {
            return _mapper.Map<Customer>(CustomerInfoProvider.GetCustomerInfo(customerId));
        }

        public User GetCurrentUser()
        {
            return _mapper.Map<User>(MembershipContext.AuthenticatedUser);
        }

        public User GetUser(string mail)
        {
            return _mapper.Map<User>(UserInfoProvider.GetUserInfo(mail));
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

        /// <summary>
        /// Creates and saves new Customer
        /// </summary>
        /// <returns>ID of new Customer</returns>
        public int CreateCustomer(Customer customer)
        {
            var customerInfo = _mapper.Map<CustomerInfo>(customer);
            customerInfo.CustomerID = 0;
            customerInfo.Insert();
            return customerInfo.CustomerID;
        }

        public void UpdateCustomer(Customer customer)
        {
            var customerInfo = CustomerInfoProvider.GetCustomerInfo(customer.Id);

            if (customerInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(customer.Id), "Existing Customer with given Id not found");
            }

            customerInfo.CustomerFirstName = customer.FirstName;
            customerInfo.CustomerLastName = customer.LastName;
            customerInfo.CustomerEmail = customer.Email;
            customerInfo.CustomerPhone = customer.Phone;
            customerInfo.CustomerCompany = customer.Company;
            customerInfo.Update();
        }

        public void CreateUser(User user, int siteId)
        {
            var newUser = _mapper.Map<UserInfo>(user);
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
            
            userInfo.Update();
        }

        public void LinkCustomerToUser(int customerId, int userId)
        {
            var customer = CustomerInfoProvider.GetCustomerInfo(customerId);
            customer.CustomerUserID = userId;
            customer.Update();
        }
    }
}