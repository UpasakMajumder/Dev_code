using Kadena.BusinessLogic.Contracts.SSO;
using Kadena.BusinessLogic.Services;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
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

        [Fact(DisplayName = "IdentityService.TryAuthenticate() | Non null attributes")]
        public void TryAuthenticateNonNullAttributes()
        {
            var expectedResult = new Uri("/", UriKind.Relative);
            var autoMock = new AutoMocker();
            autoMock
                .GetMock<ISaml2Service>()
                .Setup(s => s.GetAttributes(It.IsAny<string>()))
                .Returns(new Dictionary<string, IEnumerable<string>>());

            var sut = autoMock.CreateInstance<IdentityService>();

            var actualResult = sut.TryAuthenticate(string.Empty);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
