using System;
using Xunit;
using Kadena.BusinessLogic.Services;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.BusinessLogic.Contracts;
using Kadena.Models;

namespace Kadena.Tests.BusinessLogic
{
    public class SettingsServiceTests : KadenaUnitTest<SettingsService>
    {
        [Theory(DisplayName = "SettingsService()")]
        [ClassData(typeof(SettingsServiceTests))]
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
