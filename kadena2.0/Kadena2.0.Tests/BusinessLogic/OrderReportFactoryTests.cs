using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Factories;
using Kadena.Container.Default;
using Kadena.Models.Orders;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class OrderReportFactoryTests : KadenaUnitTest<OrderReportFactory>
    {
        [Fact]
        public void CreateTableView()
        {
            var report = new[]
                {
                    new OrderReportViewItem()
                };
            Use(MapperBuilder.MapperInstance);

            var actualView = Sut.CreateTableView(report);

            Assert.NotEmpty(actualView.Rows);
            Assert.Equal(11, actualView.Headers.Length);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 4)]
        [InlineData(3, 5)]
        [InlineData(3, 2)]
        public void CreateReportView_ShouldMapReportToReportView(int orders, int items)
        {
            var expectedCount = orders * items;

            Use(MapperBuilder.MapperInstance);
            Setup<IDateTimeFormatter, string>(dtf => dtf.Format(It.IsAny<DateTime>()), "formatted");
            var orderDto = OrderReportTestHelper.CreateTestRecentOrders(orders, items);

            var actual = Sut.CreateReportView(orderDto);

            Assert.Equal(expectedCount, actual.Count());
        }
    }
}
