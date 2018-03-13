using Xunit;
using Moq;
using Moq.AutoMock;
using Kadena.BusinessLogic.Services.SSO;
using System;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.Tests.BusinessLogic.SSO
{
    public class Saml2RecipientValidatorTest
    {
        [Fact(DisplayName = "Saml2RecipientValidator.Validate() | True")]
        public void ValidateTrue()
        {
            var expectedResult = new Uri("https://google.com/api/login/saml2", UriKind.Absolute);
            var autoMock = new AutoMocker();
            var sut = autoMock.CreateInstance<Saml2RecipientValidator>();
            autoMock
                .GetMock<IKenticoSiteProvider>()
                .Setup(s => s.GetFullUrl())
                .Returns($"{expectedResult.Scheme}{Uri.SchemeDelimiter}{expectedResult.Host}:{expectedResult.Port}");

            var actualResult = sut.ValidateRecipient(expectedResult);

            Assert.True(actualResult);
        }

        [Fact(DisplayName = "Saml2RecipientValidator.Validate() | False")]
        public void ValidateFalse()
        {
            var expectedResult = new Uri("https://google.com/api/login/saml2", UriKind.Absolute);
            var autoMock = new AutoMocker();
            var sut = autoMock.CreateInstance<Saml2RecipientValidator>();
            autoMock
                .GetMock<IKenticoSiteProvider>()
                .Setup(s => s.GetFullUrl())
                .Returns($"{expectedResult.Scheme}{Uri.SchemeDelimiter}{expectedResult.Host}:{expectedResult.Port}");

            var actualResult = sut.ValidateRecipient(new Uri("https://google.com", UriKind.Absolute));

            Assert.False(actualResult);
        }
    }
}
