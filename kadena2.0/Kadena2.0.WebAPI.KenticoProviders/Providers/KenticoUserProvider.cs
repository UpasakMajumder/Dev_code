using AutoMapper;
using CMS.Ecommerce;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Factories;
using System;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoUserProvider : IKenticoUserProvider
    {
        public static string CustomerDefaultShippingAddresIDFieldName => "CustomerDefaultShippingAddresID";

        private readonly IKenticoLogger _logger;
        private readonly IMapper _mapper;

        public KenticoUserProvider(IKenticoLogger logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public DeliveryAddress[] GetCustomerAddresses(AddressType addressType)
        {
            var customer = ECommerceContext.CurrentCustomer;
            return GetCustomerAddresses(customer.CustomerID, addressType);
        }

        public DeliveryAddress[] GetCustomerAddresses(int customerId, AddressType addressType)
        {
            var query = AddressInfoProvider.GetAddresses(customerId);
            if (addressType != null)
            {
                query = query.Where($"AddressType ='{addressType}'");
            }
            var addresses = query.ToArray();
            return _mapper.Map<DeliveryAddress[]>(addresses);
        }

        public Customer GetCurrentCustomer()
        {
            var customer = ECommerceContext.CurrentCustomer;

            if (customer == null)
                return null;

            return CustomerFactory.CreateCustomer(customer);
        }

        public Customer GetCustomer(int customerId)
        {
            var customer = CustomerInfoProvider.GetCustomerInfo(customerId);

            if (customer == null)
                return null;

            return CustomerFactory.CreateCustomer(customer);
        }

        public bool UserCanSeePrices()
        {
            return UserInfoProvider.IsAuthorizedPerResource("Kadena_Orders", "KDA_SeePrices", SiteContext.CurrentSiteName, MembershipContext.AuthenticatedUser);
        }

        public bool UserCanSeePrices(int siteId, int userId)
        {
            var userinfo = UserInfoProvider.GetUserInfo(userId);
            var site = SiteInfoProvider.GetSiteInfo(siteId);

            if (userinfo == null || site == null)
                return false;

            return UserInfoProvider.IsAuthorizedPerResource("Kadena_Orders", "KDA_SeePrices", site.SiteName, userinfo);
        }

        public bool UserCanSeeAllOrders()
        {
            return UserInfoProvider.IsAuthorizedPerResource("Kadena_Orders", "KDA_SeeAllOrders", SiteContext.CurrentSiteName, MembershipContext.AuthenticatedUser);
        }

        public bool UserCanModifyShippingAddress()
        {
            return UserInfoProvider.IsAuthorizedPerResource("Kadena_User_Settings", "KDA_ModifyShippingAddress",
                SiteContext.CurrentSiteName, MembershipContext.AuthenticatedUser);
        }

        public User GetCurrentUser()
        {
            var user = MembershipContext.AuthenticatedUser;
            return new User
            {
                UserId = user.UserID
            };
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


        public void SetDefaultShippingAddress(int addressId)
        {
            var customer = ECommerceContext.CurrentCustomer;

            if (customer != null)
            {
                customer.SetValue(CustomerDefaultShippingAddresIDFieldName, addressId);
                CustomerInfoProvider.SetCustomerInfo(customer);
            }
        }

        public void UnsetDefaultShippingAddress()
        {
            SetDefaultShippingAddress(0);
        }
    }
}