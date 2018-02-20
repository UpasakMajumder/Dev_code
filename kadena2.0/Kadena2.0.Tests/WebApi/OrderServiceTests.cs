using Kadena.BusinessLogic.Services.Orders;
using Kadena.Models.Common;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.WebApi
{
    public class OrderServiceTests
    {
        [Fact]
        public void Service_ShouldLoadPagingSettings_WhenConfigured()
        {
            var resources = Mock.Of<IKenticoResourceService>(
                res => res.GetSettingsKey(Settings.KDA_RecentOrdersPageCapacity) == "15"
                );
            var sut = new OrderService(resources);

            Assert.Equal(15, sut.OrdersPerPage);
        }

        [Fact]
        public void Service_ShouldUseDefaultPagingSettings_WhenNotConfigured()
        {
            var resources = Mock.Of<IKenticoResourceService>();
            var sut = new OrderService(resources);

            Assert.Equal(OrderService.DefaultOrdersPerPage, sut.OrdersPerPage);
        }

        [Fact]
        public async Task GetOrdersView_ShouldLoadAll_WhenDateFilterNotSpecified()
        {
            var resources = Mock.Of<IKenticoResourceService>();
            var sut = new OrderService(resources);
            // filter args
            var fromDate = (DateTime?)null;
            var toDate = (DateTime?)null;
            var sort = string.Empty;
            var page = 1;

            // TODO: configure orderViewClient
            var expectedCount = 7;
            Assert.True(expectedCount <= OrderService.DefaultOrdersPerPage);

            var ordersView = await sut.GetOrdersView(fromDate, toDate, sort, page);
            Assert.Equal(expectedCount, ordersView.Pagination.RowsCount);
        }
    }
}
