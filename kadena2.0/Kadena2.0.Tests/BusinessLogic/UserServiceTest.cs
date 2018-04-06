using Kadena.BusinessLogic.Services;
using Kadena.Models;
using Kadena.Models.Membership;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class UserServiceTest
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
            var autoMock = new AutoMocker();
            var sut = autoMock.CreateInstance<UserService>();
            var userProvider = autoMock.GetMock<IKenticoUserProvider>();

            sut.AcceptTaC();

            userProvider.Verify(s => s.AcceptTaC(), Times.AtLeastOnce);
        }

        [Theory(DisplayName = "UserService.CheckTaC() | Enabled")]
        [MemberData(nameof(GetDates))]
        public void CheckTaCEnabled(DateTime tacDate, DateTime acceptedDate)
        {
            var expectedResult = acceptedDate < tacDate;

            var autoMock = new AutoMocker();
            var sut = autoMock.CreateInstance<UserService>();
            autoMock
                .Setup<IKenticoResourceService, bool>(s => s.GetSettingsKey<bool>(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);
            autoMock
                .Setup<IKenticoDocumentProvider, DateTime>(s => s.GetTaCValidFrom())
                .Returns(tacDate);
            autoMock
                .Setup<IKenticoUserProvider, User>(s => s.GetCurrentUser())
                .Returns(new User { TermsConditionsAccepted = acceptedDate });

            var actualResult = sut.CheckTaC();

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult.Show);
        }

        [Fact(DisplayName = "UserService.CheckTaC() | Disabled")]
        public void CheckTaCDisabled()
        {
            var autoMock = new AutoMocker();
            var sut = autoMock.CreateInstance<UserService>();
            autoMock
                .Setup<IKenticoResourceService, bool>(s => s.GetSettingsKey<bool>(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(false);
           
            var actualResult = sut.CheckTaC();

            Assert.NotNull(actualResult);
            Assert.False(actualResult.Show);
        }
    }
}
