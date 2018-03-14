using AutoMapper;
using Kadena.BusinessLogic.Contracts.SSO;
using Kadena.BusinessLogic.Services;
using Kadena.Dto.SSO;
using Kadena.Models;
using Kadena.Models.Site;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class IdentityServiceTest
    {
        [Fact(DisplayName = "IdentityService.TryAuthenticate() | Null attributes")]
        public void TryAuthenticateNullAttributes()
        {
            var expectedResult = new Uri("https://en.wikipedia.org/wiki/HTTP_403", UriKind.Absolute);
            var autoMock = new AutoMocker();
            var sut = autoMock.CreateInstance<IdentityService>();

            var actualResult = sut.TryAuthenticate(string.Empty);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "IdentityService.TryAuthenticate() | Success")]
        public void TryAuthenticateNonNullAttributes()
        {
            var expectedResult = new Uri("/", UriKind.Relative);
            var autoMock = new AutoMocker();
            autoMock
                .GetMock<ISaml2Service>()
                .Setup(s => s.GetAttributes(It.IsAny<string>()))
                .Returns(new Dictionary<string, IEnumerable<string>>());
            autoMock
                .GetMock<IKenticoSiteProvider>()
                .Setup(s => s.GetKenticoSite())
                .Returns(new KenticoSite { Id = 1 });
            autoMock
                .GetMock<IKenticoLoginProvider>()
                .Setup(s => s.SSOLogin(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(true);
            autoMock
                .GetMock<IKenticoAddressBookProvider>()
                .Setup(s => s.GetAddressesList(It.IsAny<int>()))
                .Returns(Enumerable.Empty<AddressData>().ToList());
            var mapper = autoMock.GetMock<IMapper>();
            mapper.Setup(s => s.Map<User>(It.IsAny<UserDto>())).Returns(new User());
            mapper.Setup(s => s.Map<UserDto>(It.IsAny<Dictionary<string, IEnumerable<string>>>())).Returns(new UserDto());
            mapper.Setup(s => s.Map<Customer>(It.IsAny<CustomerDto>())).Returns(new Customer());

            var sut = autoMock.CreateInstance<IdentityService>();

            var actualResult = sut.TryAuthenticate(string.Empty);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "IdentityService.TryAuthenticate() | Null user DTO")]
        public void TryAuthenticateNullUserDto()
        {
            var expectedResult = new Uri("https://en.wikipedia.org/wiki/HTTP_403", UriKind.Absolute);
            var autoMock = new AutoMocker();
            autoMock
                .GetMock<ISaml2Service>()
                .Setup(s => s.GetAttributes(It.IsAny<string>()))
                .Returns(new Dictionary<string, IEnumerable<string>>());
            autoMock
                .GetMock<IKenticoSiteProvider>()
                .Setup(s => s.GetKenticoSite())
                .Returns(new KenticoSite { Id = 1 });

            var sut = autoMock.CreateInstance<IdentityService>();

            var actualResult = sut.TryAuthenticate(string.Empty);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "IdentityService.TryAuthenticate() | User exists and null customer DTO")]
        public void TryAuthenticateUserExists()
        {
            var expectedResult = new Uri("https://en.wikipedia.org/wiki/HTTP_403", UriKind.Absolute);
            var autoMock = new AutoMocker();
            autoMock
                .GetMock<ISaml2Service>()
                .Setup(s => s.GetAttributes(It.IsAny<string>()))
                .Returns(new Dictionary<string, IEnumerable<string>>());
            autoMock
                .GetMock<IKenticoSiteProvider>()
                .Setup(s => s.GetKenticoSite())
                .Returns(new KenticoSite { Id = 1 });
            autoMock
                .GetMock<IKenticoUserProvider>()
                .Setup(s => s.GetUser(It.IsAny<string>()))
                .Returns(new User());
            var mapper = autoMock.GetMock<IMapper>();
            mapper.Setup(s => s.Map<User>(It.IsAny<UserDto>())).Returns(new User());
            mapper.Setup(s => s.Map<UserDto>(It.IsAny<Dictionary<string, IEnumerable<string>>>())).Returns(new UserDto());

            var sut = autoMock.CreateInstance<IdentityService>();

            var actualResult = sut.TryAuthenticate(string.Empty);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "IdentityService.TryAuthenticate() | Customer exists")]
        public void TryAuthenticateCustomerExists()
        {
            var expectedResult = new Uri("https://en.wikipedia.org/wiki/HTTP_403", UriKind.Absolute);
            var autoMock = new AutoMocker();
            autoMock
                .GetMock<ISaml2Service>()
                .Setup(s => s.GetAttributes(It.IsAny<string>()))
                .Returns(new Dictionary<string, IEnumerable<string>>());
            autoMock
                .GetMock<IKenticoSiteProvider>()
                .Setup(s => s.GetKenticoSite())
                .Returns(new KenticoSite { Id = 1 });
            autoMock
                .GetMock<IKenticoAddressBookProvider>()
                .Setup(s => s.GetAddressesList(It.IsAny<int>()))
                .Returns(Enumerable.Empty<AddressData>().ToList());
            autoMock
                .GetMock<IKenticoUserProvider>()
                .Setup(s => s.GetCustomer(It.IsAny<int>()))
                .Returns(new Customer());
            var mapper = autoMock.GetMock<IMapper>();
            mapper.Setup(s => s.Map<User>(It.IsAny<UserDto>())).Returns(new User());
            mapper.Setup(s => s.Map<UserDto>(It.IsAny<Dictionary<string, IEnumerable<string>>>())).Returns(new UserDto());
            mapper.Setup(s => s.Map<Customer>(It.IsAny<CustomerDto>())).Returns(new Customer());

            var sut = autoMock.CreateInstance<IdentityService>();

            var actualResult = sut.TryAuthenticate(string.Empty);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
