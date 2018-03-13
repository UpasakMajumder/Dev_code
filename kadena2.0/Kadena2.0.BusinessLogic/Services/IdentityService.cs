using Kadena.BusinessLogic.Contracts;
using System;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Dto.SSO;
using AutoMapper;
using Newtonsoft.Json;
using Kadena.BusinessLogic.Contracts.SSO;

namespace Kadena.BusinessLogic.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IKenticoLogger logger;
        private readonly IMapper mapper;
        private readonly ISaml2Service saml2Service;

        public IdentityService(IKenticoLogger logger, IMapper mapper, ISaml2Service saml2Service)
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
            this.logger = logger;
            this.mapper = mapper;
            this.saml2Service = saml2Service;
        }


        public Uri TryAuthenticate(string samlString)
        {
            var attributes = saml2Service.GetAttributes(samlString);
            if (attributes != null)
            {
                var user = mapper.Map<UserDto>(attributes);
                var customer = mapper.Map<CustomerDto>(attributes);
                var address = mapper.Map<AddressDto>(attributes);
                // create/update user
                logger.LogInfo(this.GetType().Name, "SAMLUSER", JsonConvert.SerializeObject(user));
                // update roles
                logger.LogInfo(this.GetType().Name, "SAMLCUSTOMER", JsonConvert.SerializeObject(customer));
                // authenticate in Kentico
                logger.LogInfo(this.GetType().Name, "SAMLADDRESS", JsonConvert.SerializeObject(address));
                return new Uri("/", UriKind.Relative);
            }

            return new Uri("https://en.wikipedia.org/wiki/HTTP_403", UriKind.Absolute);
        }
    }
}
