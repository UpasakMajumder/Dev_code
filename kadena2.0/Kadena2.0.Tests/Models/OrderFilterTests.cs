using Kadena.Models.Orders;
using Xunit;

namespace Kadena.Tests.Models
{
    public class OrderFilterTests
    {
        [Theory(DisplayName = "OrderFilter.TryParseOrderByExpression() | Invalid sort")]
        [InlineData("INVALID")]
        [InlineData("-ASC")]
        [InlineData(" -ASC")]
        [InlineData("prop-RANDOM")]
        public void TryParseSort_ShouldBeFalse_WhenSortIsInvalid(string invalidSort)
        {
            var filter = new OrderFilter { OrderByExpression = invalidSort };

            var isValid = filter.TryParseOrderByExpression(out OrderFilter.OrderByFields sortInfo);

            Assert.False(isValid);
        }

        [Theory(DisplayName = "OrderFilter.TryParseOrderByExpression() | Valid sort")]
        [InlineData(OrderFilter.OrderByDirection.ASC, "propA", "propA-ASC")]
        [InlineData(OrderFilter.OrderByDirection.DESC, "propB", "propB-DESC")]
        public void TryParseSort_ShouldBeTrueAndParsed_WhenSortValid(OrderFilter.OrderByDirection direction, string property, string validSort)
        {
            var filter = new OrderFilter { OrderByExpression = validSort };

            var isValid = filter.TryParseOrderByExpression(out OrderFilter.OrderByFields sortInfo);

            Assert.True(isValid);
            Assert.Equal(property, sortInfo.Property);
            Assert.Equal(direction, sortInfo.Direction);
        }
    }
}
