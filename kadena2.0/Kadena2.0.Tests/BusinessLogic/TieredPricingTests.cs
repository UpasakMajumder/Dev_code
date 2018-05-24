using Kadena.Models;
using System.Collections.Generic;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class TieredPricingTests
    {
        [Fact(DisplayName = "TieredPricingTests | Overlaping")]
        public void TestOverlaping()
        {
            var data = new List<TieredPricingRange>
            {
                new TieredPricingRange{ Quantity = 10 },
                new TieredPricingRange{ Quantity = 20  }
            };

            var errors = new List<string>();

            var result = TieredPricingRange.ValidateRanges(data, errors);

            Assert.True(result);
            Assert.Empty(errors);
        }

        [Fact(DisplayName = "TieredPricingTests | Overlaping bad")]
        public void TestOverlaping_Bad()
        {
            var data = new List<TieredPricingRange>
            {
                new TieredPricingRange{ Quantity = 10 },
                new TieredPricingRange{ Quantity = 10 }
            };

            var errors = new List<string>();

            var result = TieredPricingRange.ValidateRanges(data, errors);

            Assert.False(result);
            Assert.NotEmpty(errors);
        }

        [Fact]
        public void TestOverlaping_NoRanges()
        {
            var data = new List<TieredPricingRange>();
            var errors = new List<string>();

            var result = TieredPricingRange.ValidateRanges(data, errors, true);

            Assert.True(result);
            Assert.Empty(errors);
        }

        [Fact]
        public void TestOverlaping_OneRanges()
        {
            var data = new List<TieredPricingRange>
            {
                new TieredPricingRange{ Quantity = 1212, Price = 131 }
            };

            var errors = new List<string>();

            var result = TieredPricingRange.ValidateRanges(data, errors, true);

            Assert.True(result);
            Assert.Empty(errors);
        }

        [Fact]
        public void TestOverlaping_ZeroQuantity()
        {
            var data = new List<TieredPricingRange>
            {
                new TieredPricingRange{ Quantity = 0, Price = 131 }
            };

            var errors = new List<string>();

            var result = TieredPricingRange.ValidateRanges(data, errors, true);

            Assert.False(result);
            Assert.NotEmpty(errors);
        }


        [Fact]
        public void TestOverlaping_NegativePrice()
        {
            var data = new List<TieredPricingRange>
            {
                new TieredPricingRange{ Quantity = 10, Price = -1 }
            };

            var errors = new List<string>();

            var result = TieredPricingRange.ValidateRanges(data, errors, true);

            Assert.False(result);
            Assert.NotEmpty(errors);
        }


    }
}
