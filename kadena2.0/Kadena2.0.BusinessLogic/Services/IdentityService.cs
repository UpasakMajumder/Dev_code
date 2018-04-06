using Kadena.BusinessLogic.Contracts;
using System;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Dto.SSO;
using AutoMapper;
using Kadena.BusinessLogic.Contracts.SSO;
using Kadena.Models;
using Kadena.Models.Membership;

namespace Kadena.BusinessLogic.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IKenticoLogger logger;
        private readonly IMapper mapper;
        private readonly ISaml2Service saml2Service;
        private readonly IKenticoUserProvider userProvider;
        private readonly IKenticoSiteProvider siteProvider;
        private readonly IKenticoAddressBookProvider addressProvider;
        private readonly IRoleService roleService;
        private readonly IKenticoLoginProvider loginProvider;
        private readonly IKenticoResourceService kenticoResourceService;

        public IdentityService(IKenticoLogger logger, IMapper mapper, ISaml2Service saml2Service, IKenticoUserProvider userProvider, IKenticoSiteProvider siteProvider,
            IKenticoAddressBookProvider addressProvider, IRoleService roleService, IKenticoLoginProvider loginProvider, IKenticoResourceService kenticoResourceService)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (saml2Service == null)
            {
                throw new ArgumentNullException(nameof(saml2Service));
            }
            if (userProvider == null)
            {
                throw new ArgumentNullException(nameof(userProvider));
            }
            if (siteProvider == null)
            {
                throw new ArgumentNullException(nameof(siteProvider));
            }
            if (addressProvider == null)
            {
                throw new ArgumentNullException(nameof(addressProvider));
            }
            if (roleService == null)
            {
                throw new ArgumentNullException(nameof(roleService));
            }
            if (loginProvider == null)
            {
                throw new ArgumentNullException(nameof(loginProvider));
            }
            if (kenticoResourceService == null)
            {
                throw new ArgumentNullException(nameof(kenticoResourceService));
            }

            this.logger = logger;
            this.mapper = mapper;
            this.saml2Service = saml2Service;
            this.userProvider = userProvider;
            this.siteProvider = siteProvider;
            this.addressProvider = addressProvider;
            this.roleService = roleService;
            this.loginProvider = loginProvider;
            this.kenticoResourceService = kenticoResourceService;
        }

        public Uri TryAuthenticate(string samlString)
        {
            var attributes = saml2Service.GetAttributes(samlString);
            if (attributes != null)
            {
                var userDto = mapper.Map<UserDto>(attributes);
                var currentSiteId = siteProvider.GetKenticoSite().Id;

                var user = EnsureUpdateUser(userDto, currentSiteId);
                if (user != null)
                {
                    var customerDto = mapper.Map<CustomerDto>(attributes);
                    var customer = EnsureUpdateCustomer(customerDto, user.UserId);
                    if (customer != null)
                    {
                        var addressDto = mapper.Map<AddressDto>(attributes);
                        EnsureUpdateAddress(addressDto, customer.Id);
                    }

                    roleService.AssignSSORoles(user, currentSiteId, userDto.Roles);

                    var authenticated = loginProvider.SSOLogin(user.UserName, true);
                    if (authenticated)
                    {
                        return new Uri("/", UriKind.Relative);
                    }
                }
            }
            return new Uri(kenticoResourceService.GetLogonPageUrl(), UriKind.RelativeOrAbsolute);
        }

        private User EnsureUpdateUser(UserDto user, int currentSiteId)
        {
            var newUser = mapper.Map<User>(user);
            if (newUser == null)
            {
                logger.LogInfo(this.GetType().Name, "ENSURESAMLUSER", "User info extraction has failed.");
                return null;
            }
            var userSettings = mapper.Map<UserSettings>(user);

            var existingUser = userProvider.GetUser(newUser.UserName);
            if (existingUser == null)
            {
                newUser.IsExternal = true;
                userProvider.CreateUser(newUser, currentSiteId, userSettings);
            }
            else
            {
                newUser.UserId = existingUser.UserId;
                userProvider.UpdateUser(newUser, userSettings);
            }
            return newUser;
        }

        private Customer EnsureUpdateCustomer(CustomerDto customer, int userId)
        {
            var newCustomer = mapper.Map<Customer>(customer);
            if (newCustomer == null)
            {
                logger.LogInfo(this.GetType().Name, "ENSURESAMLCUSTOMER", "Customer info extraction has failed.");
                return null;
            }
            var existingCustomer = userProvider.GetCustomerByUser(userId);
            if (existingCustomer == null)
            {
                newCustomer.Id = userProvider.CreateCustomer(newCustomer);
                userProvider.LinkCustomerToUser(newCustomer.Id, userId);
            }
            else
            {
                newCustomer.Id = existingCustomer.Id;
                userProvider.UpdateCustomer(newCustomer);
            }

            return newCustomer;
        }

        private DeliveryAddress EnsureUpdateAddress(AddressDto address, int customerId)
        {
            var newAddress = mapper.Map<DeliveryAddress>(address);
            var existingAddresses = addressProvider.GetCustomerAddresses(customerId, AddressType.Shipping);
            if (existingAddresses.Length == 0)
            {
                addressProvider.SaveShippingAddress(newAddress, customerId);
            }
            return newAddress;
        }
    }
}
