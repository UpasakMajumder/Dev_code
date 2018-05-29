using Kadena.BusinessLogic.Services;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class DialogServiceTests : KadenaUnitTest<DialogService>
    {
        public static IEnumerable<object[]> GetDependencies()
        {
            yield return new[] {
                null,
                new Mock<IKenticoLocalizationProvider>().Object
            };
            yield return new[] {
                new Mock<IKenticoResourceService>().Object,
                null
            };
        }

        [Theory(DisplayName = "DialogService()")]
        [MemberData(nameof(GetDependencies))]
        public void DialogService(IKenticoResourceService resources, IKenticoLocalizationProvider kenticoLocalization)
        {
            Assert.Throws<ArgumentNullException>(() => new DialogService(resources, kenticoLocalization));
        }

        [Fact(DisplayName = "DialogService.GetAddressFields()")]
        public void GetAddressFields()
        {
            var actualResult = Sut.GetAddressFields();

            Assert.Collection(actualResult,
                i => Assert.Equal("customerName", i.Id),
                i => Assert.Equal("address1", i.Id),
                i => Assert.Equal("address2", i.Id),
                i => Assert.Equal("city", i.Id),
                i => Assert.Equal("state", i.Id),
                i => Assert.Equal("zip", i.Id),
                i => Assert.Equal("country", i.Id),
                i => Assert.Equal("phone", i.Id),
                i => Assert.Equal("email", i.Id));
        }
    }
}
