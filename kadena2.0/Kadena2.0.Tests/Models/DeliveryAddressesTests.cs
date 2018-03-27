using Kadena.Models;
using Kadena.Models.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.Models
{
    public class DeliveryAddressesTests : KadenaUnitTest<DeliveryAddresses>
    {
        [Fact(DisplayName = "DeliveryAddresses.GetDefaultAddressId() | Empty")]
        public void GetDefaultAddressId_ShouldReturnZero_WhenNoAddresses()
        {
            var expected = 0;

            var actual = Sut.GetDefaultAddressId();

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "DeliveryAddresses.GetDefaultAddressId() | Non empty")]
        public void GetDefaultAddressId_ShouldReturnFirstAddressId()
        {
            var addresses = GetAddresses();
            var expected = addresses.First().Id;
            var sut = Sut;
            sut.items = addresses;

            var defaultAddressId = sut.GetDefaultAddressId();

            Assert.Equal(expected, defaultAddressId);
        }

        [Fact(DisplayName = "DeliveryAddresses.CheckAddress() | Checks only one")]
        public void CheckAddress_ShouldCheckOnlyOneAddress_WhenAddressIsValid()
        {
            var addresses = GetAddresses();
            var ids = addresses.Select(a => a.Id).ToList();
            var sut = Sut;
            sut.items = addresses;

            foreach (var item in ids)
            {
                sut.CheckAddress(item);
            }
            var checkedCount = sut.items.Count(a => a.Checked);

            Assert.Equal(1, checkedCount);
        }

        [Fact(DisplayName = "DeliveryAddresses.CheckAddress() | Check non existing")]
        public void CheckAddress_ShouldCheckNoAddress_WhenAddressIsInvalid()
        {
            var addresses = GetAddresses();
            var invalidId = -10;
            var sut = Sut;
            sut.items = addresses;

            sut.CheckAddress(invalidId);
            var checkedCount = sut.items.Count(a => a.Checked);

            Assert.Equal(0, checkedCount);
        }

        [Fact(DisplayName = "DeliveryAddresses.CheckAddress() | Check existing")]
        public void CheckAddress_ShouldMoveCheckedAddressToFirstPlace()
        {
            var addresses = GetAddresses();
            var validId = addresses.Last().Id;
            var sut = Sut;
            sut.items = addresses;

            sut.CheckAddress(validId);

            Assert.True(sut.items.First().Checked);
        }

        private List<DeliveryAddress> GetAddresses()
        {
            return new List<DeliveryAddress>
            {
                new DeliveryAddress { Id = 22 },
                new DeliveryAddress { Id = 33 }
            };
        }
    }
}
