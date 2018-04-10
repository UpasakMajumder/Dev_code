using Xunit;
using Kadena.BusinessLogic.Services.SSO;
using System;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.Tests.BusinessLogic.SSO
{
    public class Saml2RecipientValidatorTest : KadenaUnitTest<Saml2RecipientValidator>
    {
        [Fact(DisplayName = "Saml2RecipientValidator.Validate() | True")]
        public void ValidateTrue()
        {
            var expectedResult = new Uri("https://google.com/api/login/saml2", UriKind.Absolute);
            Setup<IKenticoSiteProvider, string>(s => s.GetFullUrl(), $"{expectedResult.Scheme}{Uri.SchemeDelimiter}{expectedResult.Host}:{expectedResult.Port}");

            var actualResult = Sut.ValidateRecipient(expectedResult);

            Assert.True(actualResult);
        }

        [Fact(DisplayName = "Saml2RecipientValidator.Validate() | False")]
        public void ValidateFalse()
        {
            var expectedResult = new Uri("https://google.com/api/login/saml2", UriKind.Absolute);
            Setup<IKenticoSiteProvider, string>(s => s.GetFullUrl(), $"{expectedResult.Scheme}{Uri.SchemeDelimiter}{expectedResult.Host}:{expectedResult.Port}");

            var actualResult = Sut.ValidateRecipient(new Uri("https://google.com", UriKind.Absolute));

            Assert.False(actualResult);
        }
    }
}
