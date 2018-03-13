using Kadena.BusinessLogic.Contracts;
using System;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Dto.SSO;
using AutoMapper;
using Newtonsoft.Json;
using Kadena.BusinessLogic.Contracts.SSO;
using Kadena.Models;

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

        public IdentityService(IKenticoLogger logger, IMapper mapper, ISaml2Service saml2Service, IKenticoUserProvider userProvider, IKenticoSiteProvider siteProvider,
            IKenticoAddressBookProvider addressProvider)
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
            this.logger = logger;
            this.mapper = mapper;
            this.saml2Service = saml2Service;
            this.userProvider = userProvider;
            this.siteProvider = siteProvider;
            this.addressProvider = addressProvider;
        }


        public Uri TryAuthenticate(string samlString)
        {
            var attributes = saml2Service.GetAttributes(samlString);
            if (attributes != null)
            {
                var userDto = mapper.Map<UserDto>(attributes);
                var customerDto = mapper.Map<CustomerDto>(attributes);
                var addressDto = mapper.Map<AddressDto>(attributes);

                var user = mapper.Map<User>(userDto);
                var customer = mapper.Map<Customer>(customerDto);
                var existingUser = userProvider.GetUser(userDto.UserName);
                if (existingUser == null)
                {
                    var address = mapper.Map<DeliveryAddress>(addressDto);
                    user.IsExternal = true;
                    var currentSite = siteProvider.GetKenticoSite();
                    userProvider.CreateUser(user, currentSite.Id);
                    userProvider.CreateCustomer(customer);
                    userProvider.LinkCustomerToUser(customer.Id, user.UserId);
                    addressProvider.SaveShippingAddress(address, customer.Id);
                }
                else
                {
                    user.UserId = existingUser.UserId;
                    userProvider.UpdateUser(user);

                    var existingCustomer = userProvider.GetCustomer(user.UserId);
                    if (existingCustomer == null)
                    {
                        userProvider.CreateCustomer(customer);
                        userProvider.LinkCustomerToUser(customer.Id, user.UserId);
                    }
                    else
                    {
                        customer.Id = existingCustomer.Id;
                        userProvider.UpdateCustomer(customer);
                    }
                }
                // update roles
                logger.LogInfo(this.GetType().Name, "SAMLCUSTOMER", JsonConvert.SerializeObject(customer));
                // authenticate in Kentico
                return new Uri("/", UriKind.Relative);
            }

            return new Uri("https://en.wikipedia.org/wiki/HTTP_403", UriKind.Absolute);
        }
    }
}
