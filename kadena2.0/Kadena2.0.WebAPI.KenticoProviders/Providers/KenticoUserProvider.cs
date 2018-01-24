using AutoMapper;
using CMS.Ecommerce;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
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
            return _mapper.Map<DeliveryAddress[]>(query.ToArray());
        }

        public DeliveryAddress[] GetCustomerShippingAddresses(int customerId)
        {
            var addresses = AddressInfoProvider.GetAddresses(customerId)
                .Where(a => a.GetStringValue("AddressType", string.Empty) == AddressType.Shipping)
                .ToArray();

            return _mapper.Map<DeliveryAddress[]>(addresses.ToArray());
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

        public bool UserIsInCurrentSite(int userId)
        {
            var user = UserInfoProvider.GetUserInfo(userId);
            return user?.IsInSite(SiteContext.CurrentSiteName) ?? false;
        }

        public UserInfo GetUserByUserId(int userId)
        {
            return UserInfoProvider.GetUserInfo(userId);
        }
    }
}