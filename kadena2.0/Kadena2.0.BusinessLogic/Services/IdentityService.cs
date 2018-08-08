using Kadena.BusinessLogic.Contracts;
using System;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Dto.SSO;
using AutoMapper;
using Kadena.BusinessLogic.Contracts.SSO;
using Kadena.Models;
using Kadena.Models.Membership;
using System.Data.SqlClient;

namespace Kadena.BusinessLogic.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IKenticoLogger logger;
        private readonly IMapper mapper;
        private readonly ISaml2Service saml2Service;
        private readonly IKenticoUserProvider userProvider;
        private readonly IKenticoCustomerProvider customerProvider;
        private readonly IKenticoSiteProvider siteProvider;
        private readonly IKenticoAddressBookProvider addressProvider;
        private readonly IRoleService roleService;
        private readonly IKenticoLoginProvider loginProvider;
        private readonly IKenticoResourceService kenticoResourceService;
        private readonly ISettingsService settingsService;

        public IdentityService(IKenticoLogger logger,
                               IMapper mapper,
                               ISaml2Service saml2Service,
                               IKenticoUserProvider userProvider,
                               IKenticoCustomerProvider customerProvider,
                               IKenticoSiteProvider siteProvider,
                               IKenticoAddressBookProvider addressProvider,
                               IRoleService roleService,
                               IKenticoLoginProvider loginProvider,
                               IKenticoResourceService kenticoResourceService,
                               ISettingsService settingsService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.saml2Service = saml2Service ?? throw new ArgumentNullException(nameof(saml2Service));
            this.userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
            this.customerProvider = customerProvider ?? throw new ArgumentNullException(nameof(customerProvider));
            this.siteProvider = siteProvider ?? throw new ArgumentNullException(nameof(siteProvider));
            this.addressProvider = addressProvider ?? throw new ArgumentNullException(nameof(addressProvider));
            this.roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            this.loginProvider = loginProvider ?? throw new ArgumentNullException(nameof(loginProvider));
            this.kenticoResourceService = kenticoResourceService ?? throw new ArgumentNullException(nameof(kenticoResourceService));
            this.settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
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

            var existingUser = userProvider.GetUser(newUser.UserName);
            if (existingUser == null)
            {
                newUser.IsExternal = true;
                userProvider.CreateUser(newUser, currentSiteId);
            }
            else
            {
                newUser.UserId = existingUser.UserId;
                userProvider.UpdateUser(newUser);
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
            var existingCustomer = customerProvider.GetCustomerByUser(userId);
            if (existingCustomer == null)
            {
                newCustomer.Id = customerProvider.CreateCustomer(newCustomer);
                userProvider.LinkCustomerToUser(newCustomer.Id, userId);
            }
            else
            {
                newCustomer.Id = existingCustomer.Id;
                customerProvider.UpdateCustomer(newCustomer);
            }

            return newCustomer;
        }

        private DeliveryAddress EnsureUpdateAddress(AddressDto address, int customerId)
        {
            var newAddress = mapper.Map<DeliveryAddress>(address);
            if (newAddress == null)
            {
                logger.LogInfo(this.GetType().Name, "ENSURESAMLADDRESS", "Customer info extraction has failed.");
                return null;
            }
            var existingAddresses = addressProvider.GetCustomerAddresses(customerId, AddressType.Shipping);
            if (existingAddresses.Length == 0)
            {
                newAddress.CustomerId = customerId;
                try
                {
                    settingsService.SaveShippingAddress(newAddress);
                }
                catch (SqlException exc)
                {
                    logger.LogException(this.GetType().Name, exc);
                    return null;
                }
            }
            return newAddress;
        }
    }
}
