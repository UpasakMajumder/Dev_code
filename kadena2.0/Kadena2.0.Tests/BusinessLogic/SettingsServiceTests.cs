using System;
using Xunit;
using Kadena.BusinessLogic.Services;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.BusinessLogic.Contracts;
using Kadena.Models;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;
using System.Collections.Generic;

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
                               IDialogService dialogService,
                               IKadenaSettings kadenaSettings)
        {
            Assert.Throws<ArgumentNullException>(() => new SettingsService(permissions, localization, site, kenticoUsers,
                               kenticoCustomers, resources, addresses, dialogService, kadenaSettings));
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

        public static IEnumerable<object[]> GetTestAddresses()
        {
            yield return new[]
            {
                new DeliveryAddress()
            };
            yield return new[]
            {
                new DeliveryAddress{ State = null, Country = null}
            };
        }

        [Theory(DisplayName = "SettingsService.SaveShippingAddress() | Empty object")]
        [MemberData(nameof(GetTestAddresses))]
        public void SaveAddress_Empty(DeliveryAddress address)
        {
            Setup<IKenticoLocalizationProvider, IEnumerable<Country>>(s => s.GetCountries(), null);
            Setup<IKenticoLocalizationProvider, IEnumerable<State>>(s => s.GetStates(), null);
            var exception = Record.Exception(() => Sut.SaveShippingAddress(address));

            Assert.Null(exception);
        }

        [Fact(DisplayName = "SettingsService.SaveShippingAddress() | Null address")]
        public void SaveAddress_Null()
        {
            Assert.Throws<NullReferenceException>(() => Sut.SaveShippingAddress(null));
        }

        [Fact(DisplayName = "SettingsService.SaveShippingAddress() | Fill up empty ids if found")]
        public void SaveAddress_EmptyIds()
        {
            var expectedInt = 1;

            var testData = new DeliveryAddress
            {
                Country = new Country { Id = 0, Code = "country" },
                State = new State { Id = 0, StateCode = "state" },
                CustomerId = 0,
                AddressName = null,
                AddressPersonalName = null,
                CompanyName = null
            };
            Setup<IKenticoLocalizationProvider, IEnumerable<Country>>(s => s.GetCountries(),
                new[] {
                    new Country { Id = expectedInt, Code = "country" },
                    new Country { Id = 21231, Code = "country1" },
                });
            Setup<IKenticoLocalizationProvider, IEnumerable<State>>(s => s.GetStates(),
                new[] {
                    new State { Id = 12412, StateCode = "state1" },
                    new State { Id = expectedInt, StateCode = "state" },
                });
            Setup<IKenticoCustomerProvider, Customer>(s => s.GetCurrentCustomer(),
                new Customer
                {
                    FirstName = "firstname",
                    LastName = "lastname",
                    Company = "company",
                    Id = expectedInt
                });

            Sut.SaveShippingAddress(testData);

            Assert.Equal(expectedInt, testData.Country.Id);
            Assert.Equal(expectedInt, testData.State.Id);
            Assert.Equal(expectedInt, testData.CustomerId);
            Assert.NotNull(testData.AddressName);
            Assert.NotNull(testData.AddressPersonalName);
            Assert.NotNull(testData.CompanyName);
            Assert.NotEqual(0, testData.AddressName.Length);
            Assert.NotEqual(0, testData.AddressPersonalName.Length);
            Assert.NotEqual(0, testData.CompanyName.Length);
        }

        [Fact(DisplayName = "SettingsService.SaveShippingAddress() | Doesn't change state of address if properly initialized")]
        public void SaveAddress_ProperlyInitialized()
        {
            var expectedInt = 1;
            var expectedString = "someString";

            var testData = new DeliveryAddress
            {
                Country = new Country { Id = expectedInt, Code = "country" },
                State = new State { Id = expectedInt, StateCode = "state" },
                CustomerId = expectedInt,
                AddressName = expectedString,
                AddressPersonalName = expectedString,
                CompanyName = expectedString
            };
            Setup<IKenticoLocalizationProvider, IEnumerable<Country>>(s => s.GetCountries(),
                new[] {
                    new Country { Id = 1241241, Code = "country" },
                    new Country { Id = 21231, Code = "country1" },
                });
            Setup<IKenticoLocalizationProvider, IEnumerable<State>>(s => s.GetStates(),
                new[] {
                    new State { Id = 12412, StateCode = "state1" },
                    new State { Id = 124124, StateCode = "state" },
                });
            Setup<IKenticoCustomerProvider, Customer>(s => s.GetCustomer(expectedInt),
                new Customer
                {
                    FirstName = "firstname",
                    LastName = "lastname",
                    Company = "company",
                    Id = expectedInt
                });

            Sut.SaveShippingAddress(testData);

            Assert.Equal(expectedInt, testData.Country.Id);
            Assert.Equal(expectedInt, testData.State.Id);
            Assert.Equal(expectedInt, testData.CustomerId);
            Assert.Equal(expectedString, testData.AddressName);
            Assert.Equal(expectedString, testData.AddressPersonalName);
            Assert.Equal(expectedString, testData.CompanyName);
        }
    }
}
