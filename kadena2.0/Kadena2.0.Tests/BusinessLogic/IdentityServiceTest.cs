using AutoMapper;
using Kadena.BusinessLogic.Contracts.SSO;
using Kadena.BusinessLogic.Services;
using Kadena.Dto.SSO;
using Kadena.Models;
using Kadena.Models.Site;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class IdentityServiceTest : KadenaUnitTest<IdentityService>
    {
        [Fact(DisplayName = "IdentityService.TryAuthenticate() | Fail")]
        public void TryAuthenticateNullAttributes()
        {
            var expectedResult = new Uri("https://en.wikipedia.org/wiki/HTTP_403", UriKind.Absolute);
            Setup<IKenticoResourceService, string>(s => s.GetLogonPageUrl(), expectedResult.AbsoluteUri);

            var actualResult = Sut.TryAuthenticate(string.Empty);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "IdentityService.TryAuthenticate() | Success")]
        public void TryAuthenticateNonNullAttributes()
        {
            var expectedResult = new Uri("/", UriKind.Relative);
            Setup<ISaml2Service, Dictionary<string, IEnumerable<string>>>(s => s.GetAttributes(It.IsAny<string>()), new Dictionary<string, IEnumerable<string>>());
            Setup<IKenticoSiteProvider, KenticoSite>(s => s.GetKenticoSite(), new KenticoSite { Id = 1 });
            Setup<IKenticoLoginProvider, bool>(s => s.SSOLogin(It.IsAny<string>(), It.IsAny<bool>()), true);
            Setup<IKenticoAddressBookProvider, List<AddressData>>(s => s.GetAddressesList(It.IsAny<int>()), Enumerable.Empty<AddressData>().ToList());
            Setup<IMapper, User>(s => s.Map<User>(It.IsAny<UserDto>()), new User());
            Setup<IMapper, UserDto>(s => s.Map<UserDto>(It.IsAny<Dictionary<string, IEnumerable<string>>>()), new UserDto());
            Setup<IMapper, Customer>(s => s.Map<Customer>(It.IsAny<CustomerDto>()), new Customer());

            var actualResult = Sut.TryAuthenticate(string.Empty);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "IdentityService.TryAuthenticate() | Null user DTO")]
        public void TryAuthenticateNullUserDto()
        {
            var expectedResult = new Uri("https://en.wikipedia.org/wiki/HTTP_403", UriKind.Absolute);
            Setup<IKenticoResourceService, string>(s => s.GetLogonPageUrl(), expectedResult.AbsoluteUri);
            Setup<ISaml2Service, Dictionary<string, IEnumerable<string>>>(s => s.GetAttributes(It.IsAny<string>()), new Dictionary<string, IEnumerable<string>>());
            Setup<IKenticoSiteProvider, KenticoSite>(s => s.GetKenticoSite(), new KenticoSite { Id = 1 });

            var actualResult = Sut.TryAuthenticate(string.Empty);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "IdentityService.TryAuthenticate() | User exists and null customer DTO")]
        public void TryAuthenticateUserExists()
        {
            var expectedResult = new Uri("https://en.wikipedia.org/wiki/HTTP_403", UriKind.Absolute);
            Setup<IKenticoResourceService, string>(s => s.GetLogonPageUrl(), expectedResult.AbsoluteUri);
            Setup<ISaml2Service, Dictionary<string, IEnumerable<string>>>(s => s.GetAttributes(It.IsAny<string>()), new Dictionary<string, IEnumerable<string>>());
            Setup<IKenticoSiteProvider, KenticoSite>(s => s.GetKenticoSite(), new KenticoSite { Id = 1 });
            Setup<IKenticoUserProvider, User>(s => s.GetUser(It.IsAny<string>()), new User());
            Setup<IMapper, User>(s => s.Map<User>(It.IsAny<UserDto>()), new User());
            Setup<IMapper, UserDto>(s => s.Map<UserDto>(It.IsAny<Dictionary<string, IEnumerable<string>>>()), new UserDto());

            var actualResult = Sut.TryAuthenticate(string.Empty);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "IdentityService.TryAuthenticate() | Customer exists")]
        public void TryAuthenticateCustomerExists()
        {
            var expectedResult = new Uri("https://en.wikipedia.org/wiki/HTTP_403", UriKind.Absolute);
            Setup<IKenticoResourceService, string>(s => s.GetLogonPageUrl(), expectedResult.AbsoluteUri);
            Setup<ISaml2Service, Dictionary<string, IEnumerable<string>>>(s => s.GetAttributes(It.IsAny<string>()), new Dictionary<string, IEnumerable<string>>());
            Setup<IKenticoSiteProvider, KenticoSite>(s => s.GetKenticoSite(), new KenticoSite { Id = 1 });
            Setup<IKenticoAddressBookProvider, List<AddressData>>(s => s.GetAddressesList(It.IsAny<int>()), Enumerable.Empty<AddressData>().ToList());
            Setup<IKenticoUserProvider, Customer>(s => s.GetCustomer(It.IsAny<int>()), new Customer());
            Setup<IMapper, User>(s => s.Map<User>(It.IsAny<UserDto>()), new User());
            Setup<IMapper, UserDto>(s => s.Map<UserDto>(It.IsAny<Dictionary<string, IEnumerable<string>>>()), new UserDto());
            Setup<IMapper, Customer>(s => s.Map<Customer>(It.IsAny<CustomerDto>()), new Customer());

            var actualResult = Sut.TryAuthenticate(string.Empty);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
