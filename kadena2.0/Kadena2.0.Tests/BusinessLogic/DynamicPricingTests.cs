using Kadena.Models;
using System.Collections.Generic;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class DynamicPricingTests
    {
        [Theory(DisplayName = "DynamicPricingTests | IsDynamicPricingOverlaping")]
        [InlineData(10,20, 30, 40)]
        [InlineData(30, 40, 10, 20)]
        [InlineData(10, 20, 21, 40)]
        [InlineData(30, 40, 10, 21)]
        public void TestOverlaping(int item1min, int item1max, int item2min, int item2max)
        {
            var data = new List<DynamicPricingRange>
            {
                new DynamicPricingRange{ MinVal = item1min, MaxVal = item1max },
                new DynamicPricingRange{ MinVal = item2min, MaxVal = item2max }
            };

            var errors = new List<string>();

            var result = DynamicPricingRange.ValidateRanges(data, errors);

            Assert.True(result);
            Assert.Empty(errors);
        }


        [Theory(DisplayName = "DynamicPricingTests_Invalid | IsDynamicPricingOverlaping")]
        [InlineData(10, 20, 15, 30)]
        [InlineData(15, 30, 10, 20)]
        [InlineData(10, 20, 11, 19)]
        [InlineData(11, 19, 10, 20)]
        public void TestOverlaping_Invalid(int item1min, int item1max, int item2min, int item2max)
        {
            var data = new List<DynamicPricingRange>
            {
                new DynamicPricingRange{ MinVal = item1min, MaxVal = item1max },
                new DynamicPricingRange{ MinVal = item2min, MaxVal = item2max }
            };
            var errors = new List<string>();

            var result = DynamicPricingRange.ValidateRanges(data, errors, true);

            Assert.False(result);
            Assert.NotEmpty(errors);
        }

        [Fact(DisplayName = "DynamicPricingTests | IsDynamicPricingOverlaping")]
        public void TestOverlaping_NoRanges()
        {
            var data = new List<DynamicPricingRange>();
            var errors = new List<string>();

            var result = DynamicPricingRange.ValidateRanges(data, errors, true);

            Assert.True(result);
            Assert.Empty(errors);
        }

        [Fact(DisplayName = "DynamicPricingTests | IsDynamicPricingOverlaping")]
        public void TestOverlaping_OneRang()
        {
            var data = new List<DynamicPricingRange>
            {
                new DynamicPricingRange{ MinVal = 10, MaxVal = 20 }
            };
            var errors = new List<string>();

            var result = DynamicPricingRange.ValidateRanges(data, errors, true);

            Assert.True(result);
            Assert.Empty(errors);
        }
    }
}
