using Kadena.BusinessLogic.Services;
using Kadena.Models.Membership;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class UserServiceTest : KadenaUnitTest<UserService>
    {
        public static IEnumerable<object[]> GetDates()
        {
            yield return new object[]
            {
                    DateTime.Today,
                    DateTime.Today.AddDays(1)
            };
            yield return new object[]
            {
                    DateTime.Today,
                    DateTime.Today.AddDays(-1)
            };
            yield return new object[]
            {
                    DateTime.Today,
                    DateTime.Today
            };
        }

        [Fact(DisplayName = "UserService.AcceptTaC()")]
        public void AcceptTaC()
        {
            Sut.AcceptTaC();

            Verify<IKenticoUserProvider>(s => s.AcceptTaC(), Times.AtLeastOnce);
        }

        [Theory(DisplayName = "UserService.CheckTaC() | Enabled")]
        [MemberData(nameof(GetDates))]
        public void CheckTaCEnabled(DateTime tacDate, DateTime acceptedDate)
        {
            var expectedResult = acceptedDate < tacDate;

            Setup<IKenticoResourceService, bool>(s => s.GetSettingsKey<bool>(It.IsAny<string>(), It.IsAny<int>()), true);
            Setup<IKenticoDocumentProvider, DateTime>(s => s.GetTaCValidFrom(), tacDate);
            Setup<IKenticoUserProvider, User>(s => s.GetCurrentUser(), new User { TermsConditionsAccepted = acceptedDate });

            var actualResult = Sut.CheckTaC();

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult.Show);
        }

        [Fact(DisplayName = "UserService.CheckTaC() | Disabled")]
        public void CheckTaCDisabled()
        {
            Setup<IKenticoResourceService, bool>(s => s.GetSettingsKey<bool>(It.IsAny<string>(), It.IsAny<int>()), false);
           
            var actualResult = Sut.CheckTaC();

            Assert.NotNull(actualResult);
            Assert.False(actualResult.Show);
        }
    }
}
