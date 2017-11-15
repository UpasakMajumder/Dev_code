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
    public class DeliveryAddressesTests
    {
        [Fact]
        public void GetDefaultAddressId_ShouldReturnZero_WhenNoAddresses()
        {
            var expected = 0;
            var addresses = new DeliveryAddresses();

            var actual = addresses.GetDefaultAddressId();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDefaultAddressId_ShouldReturnFirstAddressId()
        {
            var addresses = GetAddresses();
            var expected = addresses.items.First().Id;

            var defaultAddressId = addresses.GetDefaultAddressId();

            Assert.Equal(expected, defaultAddressId);
        }

        [Fact]
        public void CheckAddress_ShouldCheckOnlyOneAddress_WhenAddressIsValid()
        {
            var addresses = GetAddresses();
            var ids = addresses.items.Select(a => a.Id).ToList();

            foreach (var item in ids)
            {
                addresses.CheckAddress(item);
            }
            var checkedCount = addresses.items.Count(a => a.Checked);

            Assert.Equal(1, checkedCount);
        }

        [Fact]
        public void CheckAddress_ShouldCheckNoAddress_WhenAddressIsInvalid()
        {
            var addresses = GetAddresses();
            var invalidId = -10;

            addresses.CheckAddress(invalidId);
            var checkedCount = addresses.items.Count(a => a.Checked);

            Assert.Equal(0, checkedCount);
        }

        [Fact]
        public void CheckAddress_ShouldMoveCheckedAddressToFirstPlace()
        {
            var addresses = GetAddresses();
            var validId = addresses.items.Last().Id;

            addresses.CheckAddress(validId);

            Assert.True(addresses.items.First().Checked);
        }

        private DeliveryAddresses GetAddresses()
        {
            return new DeliveryAddresses
            {
                items = new List<Kadena.Models.DeliveryAddress>
                {
                    new DeliveryAddress { Id = 22 },
                    new DeliveryAddress { Id = 33 }
                }
            };
        }
    }
}
