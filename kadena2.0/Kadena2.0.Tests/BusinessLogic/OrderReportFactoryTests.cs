using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Factories;
using Kadena.Container.Default;
using Kadena.Models;
using Kadena.Models.Orders;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Kadena.Models.Shipping;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class OrderReportFactoryTests : KadenaUnitTest<OrderReportFactory>
    {
        [Theory(DisplayName = "OrderReportFactory()")]
        [ClassData(typeof(OrderReportFactoryTests))]
        public void OrderReportFactory(IKenticoOrderProvider kenticoOrderProvider,
            IKenticoCustomerProvider kenticoCustomerProvider,
            IDateTimeFormatter dateTimeFormatter,
            IKenticoResourceService kenticoResources,
            IKenticoDocumentProvider kenticoDocumentProvider,
            IOrderReportFactoryHeaders orderReportFactoryHeaders,
            IMapper mapper)
        {
            Assert.Throws<ArgumentNullException>(() => new OrderReportFactory(kenticoOrderProvider, kenticoCustomerProvider, dateTimeFormatter,
                kenticoResources, kenticoDocumentProvider, orderReportFactoryHeaders, mapper));
        }

        [Fact]
        public void CreateTableView_NonZeroQuantity()
        {
            var report = new[]
                {
                    new OrderReportViewItem
                    {
                        Quantity = 1,
                        TrackingInfos = new TrackingInfo[]{}
                    }
                };
            Use(MapperBuilder.MapperInstance);

            var actualView = Sut.CreateTableView(report);

            Assert.NotEmpty(actualView.Rows);
        }

        [Fact]
        public void CreateTableView_ZeroQuantity()
        {
            var report = new[]
                {
                    new OrderReportViewItem
                    {
                        Quantity = 0
                    }
                };
            Use(MapperBuilder.MapperInstance);

            var actualView = Sut.CreateTableView(report);

            Assert.Empty(actualView.Rows);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 4)]
        [InlineData(3, 5)]
        [InlineData(3, 2)]
        public void CreateReportView_ShouldMapReportToReportView(int orders, int items)
        {
            var expectedCount = orders * items;

            Setup<IKenticoCustomerProvider, Customer>(s => s.GetCustomer(It.IsAny<int>()), null);
            var orderDto = OrderReportTestHelper.CreateTestRecentOrders(orders, items);

            var actual = Sut.CreateReportView(orderDto);

            Assert.Equal(expectedCount, actual.Count());
            Assert.All(actual, r => Assert.Equal(string.Empty, r.User));
        }

        public static IEnumerable<object[]> GetTestCustomer()
        {
            yield return new[] {
                new Customer{
                    FirstName = "first name",
                    LastName = "last name"
                }
            };
            yield return new[] {
                new Customer{
                    Email = "some email"
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetTestCustomer))]
        public void CreateReportView_CustomerMapping(Customer testCustomer)
        {
            Setup<IKenticoCustomerProvider, Customer>(s => s.GetCustomer(It.IsAny<int>()), testCustomer);
            var orderDto = OrderReportTestHelper.CreateTestRecentOrders(1, 1);

            var actual = Sut.CreateReportView(orderDto);

            if (string.IsNullOrWhiteSpace(testCustomer.FullName))
            {
                Assert.All(actual, r => Assert.Equal(testCustomer.Email, r.User));
            }
            else
            {
                Assert.All(actual, r => Assert.Equal(testCustomer.FullName, r.User));
            }
        }
    }
}
