using CMS.Ecommerce;
using Kadena.Models;
using System.Linq;
using CMS.SiteProvider;
using CMS.Membership;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Factories;
using CMS.DataEngine;
using System;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoUserProvider : IKenticoUserProvider
    {
        public KenticoUserProvider()
        {
        }

        public DeliveryAddress[] GetCustomerAddresses(string addressType = null)
        {
            var customer = ECommerceContext.CurrentCustomer;
            var query = AddressInfoProvider.GetAddresses(customer.CustomerID);
            if (!string.IsNullOrWhiteSpace(addressType))
            {
                query = query.Where($"AddressType ='{addressType}'");
            }
            var addresses = query.ToArray();
            return AddressFactory.CreateDeliveryAddresses(addresses);
        }

        public DeliveryAddress[] GetCustomerShippingAddresses(int customerId)
        {
            var addresses = AddressInfoProvider.GetAddresses(customerId)
                .Where(a => a.GetStringValue("AddressType", string.Empty) == AddressType.Shipping)
                .ToArray();

            return AddressFactory.CreateDeliveryAddresses(addresses);
        }
       
        public Customer GetCurrentCustomer()
        {
            var customer = ECommerceContext.CurrentCustomer;

            if (customer == null)
                return null;

            return new Customer()
            {
                Id = customer.CustomerID,
                FirstName = customer.CustomerFirstName,
                LastName = customer.CustomerLastName,
                Email = customer.CustomerEmail,
                CustomerNumber = customer.CustomerGUID.ToString(),
                Phone = customer.CustomerPhone,
                UserID = customer.CustomerUserID,
                Company = customer.CustomerCompany
            };
        }

        public Customer GetCustomer(int customerId)
        {
            var customer = CustomerInfoProvider.GetCustomerInfo(customerId);

            if (customer == null)
                return null;

            return new Customer()
            {
                Id = customer.CustomerID,
                FirstName = customer.CustomerFirstName,
                LastName = customer.CustomerLastName,
                Email = customer.CustomerEmail,
                CustomerNumber = customer.CustomerGUID.ToString(),
                Phone = customer.CustomerPhone,
                UserID = customer.CustomerUserID,
                Company = customer.CustomerCompany,
                SiteId = customer.CustomerSiteID
            };
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
    }
}