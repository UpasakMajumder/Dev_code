using AutoMapper;
using Kadena.BusinessLogic.Factories;
using Kadena.BusinessLogic.Services.Orders;
using Kadena.Container.Default;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena.Dto.ViewOrder.MicroserviceResponses;
using Kadena.Infrastructure.Contracts;
using Kadena.Infrastructure.FileConversion;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using Kadena.Models.SiteSettings;
using Kadena.Tests.OrderReports;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.WebApi
{
    public class OrderReportServiceTests
    {
        Random random = new Random();

        public class OrderFilterTests
        {
            [Theory]
            [InlineData("INVALID")]
            [InlineData("-ASC")]
            [InlineData(" -ASC")]
            [InlineData("prop-RANDOM")]
            public void TryParseSort_ShouldBeFalse_WhenSortIsInvalid(string invalidSort)
            {
                var filter = new OrderFilter { OrderByExpression = invalidSort };

                OrderFilter.OrderByFields sortInfo;
                var isValid = filter.TryParseOrderByExpression(out sortInfo);

                Assert.False(isValid);
            }

            [Theory]
            [InlineData(OrderFilter.OrderByDirection.ASC, "propA", "propA-ASC")]
            [InlineData(OrderFilter.OrderByDirection.DESC, "propB", "propB-DESC")]
            public void TryParseSort_ShouldBeTrueAndParsed_WhenSortValid(OrderFilter.OrderByDirection direction, string property, string validSort)
            {
                var filter = new OrderFilter { OrderByExpression = validSort };

                OrderFilter.OrderByFields sortInfo;
                var isValid = filter.TryParseOrderByExpression(out sortInfo);

                Assert.True(isValid);
                Assert.Equal(property, sortInfo.Property);
                Assert.Equal(direction, sortInfo.Direction);
            }
        }

        [Fact]
        public void Service_ShouldLoadPagingSettings_WhenConfigured()
        {
            var resources = Mock.Of<IKenticoResourceService>(
                res => res.GetSiteSettingsKey(Settings.KDA_RecentOrdersPageCapacity) == "15"
            );
            var sut = new OrderReportServiceBuilder()
                .WithResources(resources)
                .Build();

            Assert.Equal(15, sut.OrdersPerPage);
        }

        [Fact]
        public void Service_ShouldUseDefaultPagingSettings_WhenNotConfigured()
        {
            var sut = new OrderReportServiceBuilder()
                .Build();

            Assert.Equal(OrderReportService.DefaultCountOfOrdersPerPage, sut.OrdersPerPage);
        }



        [Fact]
        public void ConvertOrdersToView_ShouldThrow_WhenArgumentNull()
        {
            var sut = new OrderReportServiceBuilder()
                .Build();

            Assert.Throws<ArgumentNullException>(() => sut.ConvertOrdersToView(null));
        }

        [Fact]
        public void ConvertOrdersToView_ShouldFlattenOrdersToTable()
        {
            var orders = new PagedData<OrderReport>
            {
                Data = OrderReportTestHelper.CreateTestOrders(ordersCount: 1, itemsPerOrderCount: 1)
            };

            var table = new TableView();
            var reportFactory = Mock.Of<IOrderReportFactory>(orf =>
                orf.CreateTableView(orders.Data) == table
            );

            var sut = new OrderReportServiceBuilder()
                .WithOrderReportFactory(reportFactory)
                .Build();

            var view = sut.ConvertOrdersToView(orders);
            Assert.Equal(table, view);
        }

        [Fact]
        public void ConvertOrdersToView_ShouldReusePagination()
        {
            var testOrder = OrderReportTestHelper.CreateTestOrder(orderNumber: 1, itemsCount: 1);
            var orders = new PagedData<OrderReport>()
            {
                Data = new List<OrderReport> { testOrder },
                Pagination = new Pagination()
            };

            var reportFactory = Mock.Of<IOrderReportFactory>(orf =>
                orf.CreateTableView(orders.Data) == new TableView()
            );

            var sut = new OrderReportServiceBuilder()
                .WithOrderReportFactory(reportFactory)
                .Build();

            var view = sut.ConvertOrdersToView(orders);

            Assert.Equal(orders.Pagination, view.Pagination);
        }



        [Fact]
        public void GetOrders_ShouldUseCurrentSite()
        {
            var currentSite = "test_site";
            var siteProvider = Mock.Of<IKenticoSiteProvider>(sp => sp.GetCurrentSiteCodeName() == currentSite);
            var sut = new OrderReportServiceBuilder()
                .WithSiteProvider(siteProvider)
                .BuildSpy<OrderService_GetOrdersForSiteSpy>();
            var page = 2;
            var filter = new OrderFilter();

            sut.GetOrders(page, filter);

            Assert.Equal(currentSite, sut.Site);
            Assert.Equal(page, sut.Page);
            Assert.Equal(filter, sut.Filter);
        }

        [Fact]
        public async Task GetOrdersForSite_ShouldReturnEmptyResult_WhenMicroserviceReturnsNullData()
        {
            var orderViewClient = new Mock<IOrderViewClient>();
            orderViewClient
                .Setup(ovc => ovc.GetOrders(It.IsAny<OrderListFilter>()))
                .ReturnsAsync(new BaseResponseDto<OrderListDto>
                {
                    Payload = null
                });
            var sut = new OrderReportServiceBuilder()
                .WithOrderViewClient(orderViewClient.Object)
                .Build();

            var result = await sut.GetOrders(1, new OrderFilter());

            Assert.Empty(result.Data);
        }

        [Fact]
        public void GetOrdersForSite_ShouldValidateArgumentsAndThrow_WhenInvalidPage()
        {
            var sut = new OrderReportServiceBuilder()
                .Build();
            var invalidPageNumber = OrderReportService.FirstPageNumber - 1;
            var filter = new OrderFilter();

            Func<Task> action = () => sut.GetOrdersForSite("test_site", invalidPageNumber, filter);

            Assert.ThrowsAsync<ArgumentException>("page", action);
        }

        [Fact]
        public async Task GetOrdersForSite_ShouldValidateFilter()
        {
            var sut = new OrderReportServiceBuilder()
                .BuildSpy<OrderReportService_ValidateFilterSpy>();

            try
            {
                await sut.GetOrdersForSite("test_site", OrderReportService.FirstPageNumber, new OrderFilter());
            }
            catch(NullReferenceException)
            {
                // ignore missing implementation
            }

            Assert.True(sut.ValidateFilterCalled);
        }

        [Fact]
        public async Task GetOrdersForSite_ShouldPassArgumentsToMicroserviceClient()
        {
            var orderViewClient = new OrderViewClient_GetOrdersByFilterSpy();
            var sut = new OrderReportServiceBuilder()
                .WithOrderViewClient(orderViewClient)
                .Build();
            var page = 2;
            var currentSite = "test_site";
            var filter = new OrderFilter
            {
                FromDate = new DateTime(),
                ToDate = new DateTime(),
                OrderByExpression = $"{OrderReportService.SortableByOrderDate}-{OrderFilter.OrderByDirection.DESC}"
            };
            var expectedOrderListFilter = new OrderListFilter
            {
                SiteName = currentSite,
                ItemsPerPage = sut.OrdersPerPage,
                PageNumber = page,
                DateFrom = filter.FromDate,
                DateTo = filter.ToDate.Value.AddDays(1),
                OrderBy = OrderReportService.SortableByOrderDate,
                OrderByDescending = true
            };

            try
            {
                await sut.GetOrdersForSite(currentSite, page, filter);
            }
            catch (NullReferenceException)
            {
                // ignore missing implementation
            }

            Assert.Equal(expectedOrderListFilter, orderViewClient.Filter);
        }        


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ValidateFilter_ShouldNotThrow_WhenFilterHasEmptyOrderByExpression(string orderBy)
        {
            var sut = new OrderReportServiceBuilder()
                .Build();
            var filterWithEmptySort = new OrderFilter { OrderByExpression = orderBy };

            sut.ValidateFilter(filterWithEmptySort);

            Assert.True(true);
        }

        [Theory]
        [InlineData("wrong-sort-expression")]
        [InlineData("not_supported_property-ASC")]
        public void ValidateFilter_ShouldThrow_WhenFilterHasInvalidOrderByExpression(string orderBy)
        {
            var sut = new OrderReportServiceBuilder()
                .Build();
            var filterWithInvalidSort = new OrderFilter { OrderByExpression = orderBy };

            Action action = () => sut.ValidateFilter(filterWithInvalidSort);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ValidateFilter_ShouldThrow_WhenFilterHasInvalidDateRange()
        {
            var pastDate = new DateTime(2010, 1, 1);
            var futureDate = new DateTime(2010, 2, 1);

            var sut = new OrderReportServiceBuilder()
                .Build();
            var filterWithInvalidDateRange = new OrderFilter
            {
                FromDate = futureDate,
                ToDate = pastDate
            };

            Action action = () => sut.ValidateFilter(filterWithInvalidDateRange);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void GetOrdersExport_ShouldUseCurrentSite()
        {
            var currentSite = "test_site";
            var siteProvider = Mock.Of<IKenticoSiteProvider>(sp => sp.GetCurrentSiteCodeName() == currentSite);
            var sut = new OrderReportServiceBuilder()
                .WithSiteProvider(siteProvider)
                .BuildSpy<OrderService_GetOrdersExportForSiteSpy>();
            var filter = new OrderFilter();

            sut.GetOrdersExport(filter);

            Assert.Equal(currentSite, sut.Site);
            Assert.Equal(filter, sut.Filter);
        }

        [Fact]
        public async Task GetOrdersExportForSite_ShouldValidateFilter()
        {
            var sut = new OrderReportServiceBuilder()
                .BuildSpy<OrderReportService_ValidateFilterSpy>();

            try
            {
                await sut.GetOrdersExportForSite("test_site", new OrderFilter());
            }
            catch (NullReferenceException)
            {
                // ignore missing implementation of rest dependcies
            }

            Assert.True(sut.ValidateFilterCalled);
        }

        [Fact]
        public async Task GetOrdersForSite_ShouldConfigureDateFilterAsInclusive()
        {
            var inputDateFrom = new DateTime(2017, 3, 21);
            var inputDateTo = new DateTime(2017, 4, 25);
            var expectedDateFrom = new DateTime(2017, 3, 21);
            var expectedDateTo = new DateTime(2017, 4, 26);
            var inputFilter = new OrderFilter
            {
                FromDate = inputDateFrom,
                ToDate = inputDateTo
            };

            var actualFilter = new OrderListFilter();

            var orderViewClient = new Mock<IOrderViewClient>();
            orderViewClient
                .Setup(ovc => ovc.GetOrders(It.IsAny<OrderListFilter>()))
                .Returns<OrderListFilter>((filt) => 
                {
                    actualFilter = filt;
                    return Task.FromResult(new BaseResponseDto<OrderListDto>());
                });

            var sut = new OrderReportServiceBuilder()
                .WithOrderViewClient(orderViewClient.Object)
                .Build();

            await sut.GetOrdersExportForSite("test_site", inputFilter);

            Assert.Equal(expectedDateFrom, actualFilter.DateFrom);
            Assert.Equal(expectedDateTo, actualFilter.DateTo);
        }

        [Fact]
        public async Task GetOrdersExportForSite_ShouldCreateExportFileResult()
        {
            var orders = new BaseResponseDto<OrderListDto>()
            {
                Payload = new OrderListDto { Orders = OrderReportTestHelper.CreateTestRecentOrders(1, 1) }
            };
            var orderViewClient = CreateOrderViewClientReturning(orders);

            var dummyFileData = new byte[] { 1,2,3 };
            var convert = Mock.Of<IExcelConvert>(ec =>
                ec.Convert(It.IsAny<Table>()) == dummyFileData);
            var sut = new OrderReportServiceBuilder()
                .WithOrderViewClient(orderViewClient)
                .WithExcelConvert(convert)
                .Build();

            var result = await sut.GetOrdersExportForSite("test_site", new OrderFilter());

            Assert.Equal("export.xlsx", result.Name);
            Assert.Equal(ContentTypes.Xlsx, result.Mime);
            Assert.Equal(dummyFileData, result.Data);
        }

        [Fact]
        public async Task GetOrdersExportForSite_ShouldReturnEmptyResult_WhenMicroserviceReturnsNull()
        {
            var emptyFileLength = 3;
            var orders = new BaseResponseDto<OrderListDto>()
            {
                Payload = null
            };
            var orderViewClient = CreateOrderViewClientReturning(orders);
            var convert = Mock.Of<IExcelConvert>(ec =>
                ec.Convert(It.IsAny<Table>()) == new byte[emptyFileLength]);
            var sut = new OrderReportServiceBuilder()
                .WithOrderViewClient(orderViewClient)
                .WithExcelConvert(convert)
                .Build();

            var result = await sut.GetOrdersExportForSite("test_site", new OrderFilter());

            Assert.Equal(emptyFileLength, result.Data.Length);
        }

        [Fact]
        public async Task GetOrdersExportForSite_ShouldCreateTableForConversion()
        {
            // expected values to be passed to excelconvert
            var expected = new TableView()
            {
                Rows = new[]
                {
                    new Kadena.Models.Common.TableRow
                    {
                        Items = Enumerable.Range(1, 7).Cast<object>().ToArray(),
                    },
                    new Kadena.Models.Common.TableRow
                    {
                        Items = Enumerable.Range(12, 9).Cast<object>().ToArray(),
                    }
                }
            };

            // configure fake responses to prevent null exceptions all over the place
            var orders = new BaseResponseDto<OrderListDto>()
            {
                Payload = new OrderListDto { Orders = OrderReportTestHelper.CreateTestRecentOrders(1, 1) }
            };
            var orderViewClient = CreateOrderViewClientReturning(orders);

            var convert = new ExcelConvertFake();

            var order = new OrderReport();
            var orderFactory = Mock.Of<IOrderReportFactory>(orf =>
                orf.Create(It.IsAny<RecentOrderDto>()) == order &&
                orf.CreateTableView(It.IsAny<IEnumerable<OrderReport>>()) == expected
            );

            var sut = new OrderReportServiceBuilder()
                .WithOrderViewClient(orderViewClient)
                .WithOrderReportFactory(orderFactory)
                .WithExcelConvert(convert)
                .Build();

            // act
            await sut.GetOrdersExportForSite("test_site", new OrderFilter());

            // assert
            Assert.True(AreEqual(expected, convert.TableToConvert));
        }

        [Fact]
        public async Task GetOrdersExportForSite_ShouldPassArgumentsToMicroserviceClient_WhenFilterSpecified()
        {
            var orderViewClient = new OrderViewClient_GetOrdersByFilterSpy();
            var sut = new OrderReportServiceBuilder()
                .WithOrderViewClient(orderViewClient)
                .Build();
            var currentSite = "test_site";
            var filterSpecified = new OrderFilter
            {
                FromDate = new DateTime(),
                ToDate = new DateTime(),
                OrderByExpression = $"{OrderReportService.SortableByOrderDate}-{OrderFilter.OrderByDirection.DESC}"
            };
            var expectedOrderListFilter = new OrderListFilter
            {
                SiteName = currentSite,
                DateFrom = filterSpecified.FromDate,
                DateTo = filterSpecified.ToDate.Value.AddDays(1),
                OrderBy = OrderReportService.SortableByOrderDate,
                OrderByDescending = true
            };

            try
            {
                await sut.GetOrdersExportForSite(currentSite, filterSpecified);
            }
            catch (NullReferenceException)
            {
                // ignore missing implementation
            }

            Assert.Equal(expectedOrderListFilter, orderViewClient.Filter);
        }

        [Fact]
        public async Task GetOrdersExportForSite_ShouldPassArgumentsToMicroserviceClient_WhenFilterEmpty()
        {
            var orderViewClient = new OrderViewClient_GetOrdersByFilterSpy();
            var sut = new OrderReportServiceBuilder()
                .WithOrderViewClient(orderViewClient)
                .Build();
            var currentSite = "test_site";
            var filterEmpty = new OrderFilter { };
            var expectedOrderListFilter = new OrderListFilter
            {
                SiteName = currentSite
            };

            try
            {
                await sut.GetOrdersExportForSite(currentSite, filterEmpty);
            }
            catch (NullReferenceException)
            {
                // ignore missing implementation
            }

            Assert.Equal(expectedOrderListFilter, orderViewClient.Filter);
        }

        private bool AreEqual(TableView t1, Table t2)
        {
            if (t1 == null)
            {
                throw new ArgumentNullException(nameof(t1));
            }
            if (t2 == null)
            {
                throw new ArgumentNullException(nameof(t2));
            }

            if (t1.Rows.Length != t2.Rows.Length)
            {
                return false;
            }

            for (int row = 0; row < t1.Rows.Length; row++)
            {
                if (t1.Rows[row].Items.Length != t2.Rows[row].Items.Length)
                {
                    return false;
                }

                for (int cell = 0; cell < t1.Rows[row].Items.Length; cell++)
                {
                    if (t1.Rows[row].Items[cell] != t2.Rows[row].Items[cell])
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        private class OrderService_GetOrdersForSiteSpy : OrderReportService
        {
            public OrderService_GetOrdersForSiteSpy(
                IKenticoResourceService kenticoResources,
                IKenticoSiteProvider kenticoSiteProvider,
                IOrderViewClient orderViewClient,
                IExcelConvert excelConvert,
                IOrderReportFactory orderReportFactory,
                IMapper mapper)
                : base(kenticoResources, kenticoSiteProvider,
                      orderViewClient, excelConvert, orderReportFactory, mapper)
            {
            }

            public string Site { get; private set; }
            public int Page { get; private set; }
            public OrderFilter Filter { get; private set; }

            public override Task<PagedData<OrderReport>> GetOrdersForSite(string site, int page, OrderFilter filter)
            {
                Site = site;
                Page = page;
                Filter = filter;

                return base.GetOrdersForSite(site, page, filter);
            }
        }

        private class OrderService_GetOrdersExportForSiteSpy : OrderReportService
        {
            public OrderService_GetOrdersExportForSiteSpy(
                IKenticoResourceService kenticoResources,
                IKenticoSiteProvider kenticoSiteProvider,
                IOrderViewClient orderViewClient,
                IExcelConvert excelConvert,
                IOrderReportFactory orderReportFactory,
                IMapper mapper)
                : base(kenticoResources, kenticoSiteProvider,
                      orderViewClient, excelConvert, orderReportFactory, mapper)
            {
            }

            public string Site { get; private set; }
            public OrderFilter Filter { get; private set; }

            public override Task<FileResult> GetOrdersExportForSite(string site, OrderFilter filter)
            {
                Site = site;
                Filter = filter;

                return base.GetOrdersExportForSite(site, filter);
            }
        }

        private class OrderReportService_ValidateFilterSpy : OrderReportService
        {
            public OrderReportService_ValidateFilterSpy(
                IKenticoResourceService kenticoResources,
                IKenticoSiteProvider kenticoSiteProvider,
                IOrderViewClient orderViewClient,
                IExcelConvert excelConvert,
                IOrderReportFactory orderReportFactory,
                IMapper mapper)
                : base(kenticoResources, kenticoSiteProvider, 
                      orderViewClient, excelConvert, orderReportFactory, mapper)
            {
            }

            public bool ValidateFilterCalled { get; private set; }

            public override void ValidateFilter(OrderFilter filter)
            {
                ValidateFilterCalled = true;

                base.ValidateFilter(filter);
            }
        }

        private class OrderReportServiceBuilder
        {
            IKenticoResourceService kenticoResources;
            IKenticoSiteProvider kenticoSiteProvider;
            IOrderViewClient orderViewClient;
            IExcelConvert excelConvert;
            IOrderReportFactory orderReportFactory;

            public OrderReportService Build()
            {
                return new OrderReportService(
                    kenticoResources ?? Mock.Of<IKenticoResourceService>(),
                    kenticoSiteProvider ?? Mock.Of<IKenticoSiteProvider>(),
                    orderViewClient ?? Mock.Of<IOrderViewClient>(),
                    excelConvert ?? Mock.Of<IExcelConvert>(),
                    orderReportFactory ?? Mock.Of<IOrderReportFactory>(),
                    MapperBuilder.MapperInstance
                );
            }

            public T BuildSpy<T>() where T : OrderReportService 
            {
                return (T)Activator.CreateInstance(typeof(T),
                    kenticoResources ?? Mock.Of<IKenticoResourceService>(),
                    kenticoSiteProvider ?? Mock.Of<IKenticoSiteProvider>(),
                    orderViewClient ?? Mock.Of<IOrderViewClient>(),
                    excelConvert ?? Mock.Of<IExcelConvert>(),
                    orderReportFactory ?? Mock.Of<IOrderReportFactory>(),
                    MapperBuilder.MapperInstance
                );
            }

            public OrderReportServiceBuilder WithResources(IKenticoResourceService kenticoResources)
            {
                this.kenticoResources = kenticoResources;
                return this;
            }

            public OrderReportServiceBuilder WithSiteProvider(IKenticoSiteProvider kenticoSiteProvider)
            {
                this.kenticoSiteProvider = kenticoSiteProvider;
                return this;
            }

            public OrderReportServiceBuilder WithOrderViewClient(IOrderViewClient orderViewClient)
            {
                this.orderViewClient = orderViewClient;
                return this;
            }

            public OrderReportServiceBuilder WithExcelConvert(IExcelConvert excelConvert)
            {
                this.excelConvert = excelConvert;
                return this;
            }

            public OrderReportServiceBuilder WithOrderReportFactory(IOrderReportFactory orderReportFactory)
            {
                this.orderReportFactory = orderReportFactory;
                return this;
            }
        }

        private IOrderViewClient CreateOrderViewClientReturning(BaseResponseDto<OrderListDto> response)
        {
            return Mock.Of<IOrderViewClient>(ovc => 
                ovc.GetOrders(It.IsAny<OrderListFilter>()) == Task.FromResult(response)
            );
        }

        class OrderViewClient_GetOrdersByFilterSpy : IOrderViewClient
        {
            public Task<BaseResponseDto<GetOrderByOrderIdResponseDTO>> GetOrderByOrderId(string orderId)
            {
                throw new NotImplementedException();
            }

            public Task<BaseResponseDto<OrderListDto>> GetOrders(string siteName, int pageNumber, int quantity)
            {
                throw new NotImplementedException();
            }

            public Task<BaseResponseDto<OrderListDto>> GetOrders(int customerId, int pageNumber, int quantity)
            {
                throw new NotImplementedException();
            }

            public Task<BaseResponseDto<OrderListDto>> GetOrders(string siteName, int pageNumber, int quantity, int campaignID, string orderType)
            {
                throw new NotImplementedException();
            }

            public Task<BaseResponseDto<OrderListDto>> GetOrders(int customerId, int pageNumber, int quantity, int campaignID, string orderType)
            {
                throw new NotImplementedException();
            }

            public OrderListFilter Filter { get; private set; }

            public Task<BaseResponseDto<OrderListDto>> GetOrders(OrderListFilter filter)
            {
                Filter = filter;

                return null;
            }
        }

        private class ExcelConvertFake : IExcelConvert
        {
            public Table TableToConvert { get; private set; }

            public byte[] Convert(Table data)
            {
                TableToConvert = data;

                return null;
            }
        }
    }
}
