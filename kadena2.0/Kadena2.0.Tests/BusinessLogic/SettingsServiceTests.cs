using System;
using Xunit;
using Moq;
using Kadena.BusinessLogic.Services;
using System.Collections.Generic;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.BusinessLogic.Contracts;
using Kadena.Models;

namespace Kadena.Tests.BusinessLogic
{
    public class SettingsServiceTests : KadenaUnitTest<SettingsService>
    {
        public static IEnumerable<object[]> GetDependencies()
        {
            yield return new object[] {
                null,
                new Mock<IKenticoLocalizationProvider>().Object,
                new Mock<IKenticoSiteProvider>().Object,
                new Mock<IKenticoUserProvider>().Object,
                new Mock<IKenticoCustomerProvider>().Object,
                new Mock<IKenticoResourceService>().Object,
                new Mock<IKenticoAddressBookProvider>().Object,
                new Mock<IDialogService>().Object,
            };
            yield return new object[] {
                new Mock<IKenticoPermissionsProvider>().Object,
                null,
                new Mock<IKenticoSiteProvider>().Object,
                new Mock<IKenticoUserProvider>().Object,
                new Mock<IKenticoCustomerProvider>().Object,
                new Mock<IKenticoResourceService>().Object,
                new Mock<IKenticoAddressBookProvider>().Object,
                new Mock<IDialogService>().Object,
            };
            yield return new object[] {
                new Mock<IKenticoPermissionsProvider>().Object,
                new Mock<IKenticoLocalizationProvider>().Object,
                null,
                new Mock<IKenticoUserProvider>().Object,
                new Mock<IKenticoCustomerProvider>().Object,
                new Mock<IKenticoResourceService>().Object,
                new Mock<IKenticoAddressBookProvider>().Object,
                new Mock<IDialogService>().Object,
            };
            yield return new object[] {
                new Mock<IKenticoPermissionsProvider>().Object,
                new Mock<IKenticoLocalizationProvider>().Object,
                new Mock<IKenticoSiteProvider>().Object,
                null,
                new Mock<IKenticoCustomerProvider>().Object,
                new Mock<IKenticoResourceService>().Object,
                new Mock<IKenticoAddressBookProvider>().Object,
                new Mock<IDialogService>().Object,
            };
            yield return new object[] {
                new Mock<IKenticoPermissionsProvider>().Object,
                new Mock<IKenticoLocalizationProvider>().Object,
                new Mock<IKenticoSiteProvider>().Object,
                new Mock<IKenticoUserProvider>().Object,
                null,
                new Mock<IKenticoResourceService>().Object,
                new Mock<IKenticoAddressBookProvider>().Object,
                new Mock<IDialogService>().Object,
            };
            yield return new object[] {
                new Mock<IKenticoPermissionsProvider>().Object,
                new Mock<IKenticoLocalizationProvider>().Object,
                new Mock<IKenticoSiteProvider>().Object,
                new Mock<IKenticoUserProvider>().Object,
                new Mock<IKenticoCustomerProvider>().Object,
                null,
                new Mock<IKenticoAddressBookProvider>().Object,
                new Mock<IDialogService>().Object,
            };
            yield return new object[] {
                new Mock<IKenticoPermissionsProvider>().Object,
                new Mock<IKenticoLocalizationProvider>().Object,
                new Mock<IKenticoSiteProvider>().Object,
                new Mock<IKenticoUserProvider>().Object,
                new Mock<IKenticoCustomerProvider>().Object,
                new Mock<IKenticoResourceService>().Object,
                null,
                new Mock<IDialogService>().Object,
            };
            yield return new object[] {
                new Mock<IKenticoPermissionsProvider>().Object,
                new Mock<IKenticoLocalizationProvider>().Object,
                new Mock<IKenticoSiteProvider>().Object,
                new Mock<IKenticoUserProvider>().Object,
                new Mock<IKenticoCustomerProvider>().Object,
                new Mock<IKenticoResourceService>().Object,
                new Mock<IKenticoAddressBookProvider>().Object,
                null,
            };
        }

        [Theory(DisplayName = "SettingsService()")]
        [MemberData(nameof(GetDependencies))]
        public void SettingsService(IKenticoPermissionsProvider permissions,
                               IKenticoLocalizationProvider localization,
                               IKenticoSiteProvider site,
                               IKenticoUserProvider kenticoUsers,
                               IKenticoCustomerProvider kenticoCustomers,
                               IKenticoResourceService resources,
                               IKenticoAddressBookProvider addresses,
                               IDialogService dialogService)
        {
            Assert.Throws<ArgumentNullException>(() => new SettingsService(permissions, localization, site, kenticoUsers,
                               kenticoCustomers, resources, addresses, dialogService));
        }

        [Fact(DisplayName = "SettingsService.GetAddresses() | Current customer not initialized")]
        public void GetAddresses_CustomerNull()
        {
            Assert.Throws<NullReferenceException>(() => Sut.GetAddresses());
        }

        [Fact(DisplayName = "SettingsService.GetAddresses() | Success")]
        public void GetAddresses_Success()
        {
            Setup<IKenticoCustomerProvider, Customer>(s => s.GetCurrentCustomer(), new Customer());

            var actualResult = Sut.GetAddresses();

            Assert.NotNull(actualResult);
        }
    }
}
