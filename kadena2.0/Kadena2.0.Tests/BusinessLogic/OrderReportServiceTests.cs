using Kadena.BusinessLogic.Factories;
using Kadena.BusinessLogic.Services.Orders;
using Kadena.Container.Default;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena.Infrastructure.Contracts;
using Kadena.Infrastructure.FileConversion;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class OrderReportServiceTests : KadenaUnitTest<OrderReportService>
    {
        [Fact]
        public void Service_ShouldLoadPagingSettings_WhenConfigured()
        {
            Setup<IKenticoResourceService, string>(res => res.GetSiteSettingsKey(Settings.KDA_RecentOrdersPageCapacity), "15");

            var actual = Sut.OrdersPerPage;

            Assert.Equal(15, actual);
        }

        [Fact]
        public void Service_ShouldUseDefaultPagingSettings_WhenNotConfigured()
        {
            var expected = OrderReportService.DefaultCountOfOrdersPerPage;

            var actual = Sut.OrdersPerPage;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ConvertOrdersToView_ShouldMapOrdersToTableView()
        {

            var tableView = new TableView
            {
                Pagination = new Pagination()
            };
            Setup<IOrderViewClient, Task<BaseResponseDto<OrderListDto>>>(s => s.GetOrders(It.IsAny<OrderListFilter>()),
                Task.FromResult(new BaseResponseDto<OrderListDto>()));
            Setup<IOrderReportFactory, TableView>(orf => orf.CreateTableView(It.IsAny<IEnumerable<OrderReportViewItem>>()), tableView);

            var actual = await Sut.ConvertOrdersToView(1, new OrderFilter());

            Assert.Equal(tableView, actual);
            Assert.Equal(tableView.Pagination, actual.Pagination);
        }

        [Fact]
        public async Task GetOrders_ShouldGetOrdersForCurrentSite()
        {
            var currentSite = "test_site";
            Setup<IKenticoSiteProvider, string>(sp => sp.GetCurrentSiteCodeName(), currentSite);
            var actualFilter = new OrderListFilter();
            Setup<IOrderViewClient, OrderListFilter, Task<BaseResponseDto<OrderListDto>>>(s => s.GetOrders(It.IsAny<OrderListFilter>())
                , f =>
                {
                    actualFilter = f;
                    return Task.FromResult(new BaseResponseDto<OrderListDto>());
                });
            Setup<IOrderReportFactory, TableView>(s => s.CreateTableView(It.IsAny<IEnumerable<OrderReportViewItem>>()), new TableView());
            var page = 2;

            await Sut.ConvertOrdersToView(page, new OrderFilter());

            Assert.Equal(currentSite, actualFilter.SiteName);
            Assert.Equal(page, actualFilter.PageNumber);
        }

        [Fact]
        public async Task GetOrdersForSite_ShouldThrow_WhenInvalidPage()
        {
            var invalidPageNumber = OrderReportService.FirstPageNumber - 1;

            Task action() => Sut.GetOrdersForSite("test_site", invalidPageNumber, new OrderFilter());

            await Assert.ThrowsAsync<ArgumentException>("page", action);
        }

        [Fact]
        public async Task GetOrdersForSite_ShouldValidateFilter()
        {
            var someInvalidFilter = GetInvalidDateFilter();

            Task action() => Sut.GetOrdersForSite("test_site", OrderReportService.FirstPageNumber
                 , someInvalidFilter);

            await Assert.ThrowsAsync<ArgumentException>("filter", action);
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

            Setup<IOrderViewClient, OrderListFilter, Task<BaseResponseDto<OrderListDto>>>(s => s.GetOrders(It.IsAny<OrderListFilter>())
                , f =>
                {
                    actualFilter = f;
                    return Task.FromResult(new BaseResponseDto<OrderListDto>());
                });

            await Sut.GetOrdersExportForSite("test_site", inputFilter);

            Assert.Equal(expectedDateFrom, actualFilter.DateFrom);
            Assert.Equal(expectedDateTo, actualFilter.DateTo);
        }

        [Fact]
        public async Task GetOrdersForSite_ShouldPassArgumentsToMicroserviceClient()
        {
            var actualFilter = new OrderListFilter();
            Setup<IOrderViewClient, OrderListFilter, Task<BaseResponseDto<OrderListDto>>>(s => s.GetOrders(It.IsAny<OrderListFilter>())
                , f =>
                    {
                        actualFilter = f;
                        return Task.FromResult(new BaseResponseDto<OrderListDto>());
                    });
            var sut = Sut;
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

            await sut.GetOrdersForSite(currentSite, page, filter);

            Assert.Equal(expectedOrderListFilter, actualFilter);
        }

        [Theory]
        [InlineData("wrong-orderBy-expression")]
        [InlineData("not_supported_property-ASC")]
        public async Task ValidateFilter_ShouldThrow_WhenFilterHasInvalidOrderByExpression(string orderBy)
        {
            var filterWithInvalidSort = new OrderFilter { OrderByExpression = orderBy };

            Task action() => Sut.ConvertOrdersToView(1, filterWithInvalidSort);

            await Assert.ThrowsAsync<ArgumentException>("filter", action);
        }

        private OrderFilter GetInvalidDateFilter() =>
            new OrderFilter
            {
                FromDate = new DateTime(2020, 1, 1),
                ToDate = new DateTime(1910, 1, 1)
            };

        [Fact]
        public async Task ValidateFilter_ShouldThrow_WhenFilterHasInvalidDateRange()
        {
            var filterWithInvalidDateRange = GetInvalidDateFilter();

            Task action() => Sut.ConvertOrdersToView(1, filterWithInvalidDateRange);

            await Assert.ThrowsAsync<ArgumentException>("filter", action);
        }

        [Fact]
        public void GetOrdersExport_ShouldGetOrdersForCurrentSite()
        {
            var currentSite = "test_site";
            Setup<IKenticoSiteProvider, string>(sp => sp.GetCurrentSiteCodeName(), currentSite);
            var actualFilter = new OrderListFilter();
            Setup<IOrderViewClient, OrderListFilter, Task<BaseResponseDto<OrderListDto>>>(s => s.GetOrders(It.IsAny<OrderListFilter>())
                , f =>
                {
                    actualFilter = f;
                    return Task.FromResult(new BaseResponseDto<OrderListDto>());
                });
            var sut = Sut;

            sut.GetOrdersExport(new OrderFilter());

            Assert.Equal(currentSite, actualFilter.SiteName);
        }

        [Fact]
        public async Task GetOrdersExportForSite_ShouldValidateFilter()
        {
            var someInvalidFilter = GetInvalidDateFilter();

            Task action() => Sut.GetOrdersExportForSite("test_site", someInvalidFilter);

            await Assert.ThrowsAsync<ArgumentException>("filter", action);
        }

        [Fact]
        public async Task GetOrdersExportForSite_ShouldCreateExportFileResult()
        {
            var orders = new BaseResponseDto<OrderListDto>()
            {
                Success = true,
                Payload = new OrderListDto { Orders = OrderReportTestHelper.CreateTestRecentOrders(1, 1) }
            };
            SetupOrderViewClientReturning(orders);

            var dummyFileData = new byte[] { 1, 2, 3 };
            Setup<IExcelConvert, byte[]>(ec => ec.Convert(It.IsAny<Table>()), dummyFileData);

            var result = await Sut.GetOrdersExportForSite("test_site", new OrderFilter());

            Assert.Equal("export.xlsx", result.Name);
            Assert.Equal(ContentTypes.Xlsx, result.Mime);
            Assert.Equal(dummyFileData, result.Data);
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
                Success = true,
                Payload = new OrderListDto { Orders = OrderReportTestHelper.CreateTestRecentOrders(1, 1) }
            };
            SetupOrderViewClientReturning(orders);
            var actualResult = new Table();
            Setup<IOrderReportFactory, TableView>(orf => orf.CreateTableView(It.IsAny<IEnumerable<OrderReportViewItem>>()), expected);
            Setup<IExcelConvert, Table, byte[]>(ec => ec.Convert(It.IsAny<Table>()), t =>
              {
                  actualResult = t;
                  return null;
              });
            Use(MapperBuilder.MapperInstance);

            // act
            await Sut.GetOrdersExportForSite("test_site", new OrderFilter());

            // assert
            Assert.True(AreEqual(expected, actualResult));
        }

        [Fact]
        public async Task GetOrdersExportForSite_ShouldPassArgumentsToMicroserviceClient_WhenFilterSpecified()
        {
            var actualFilter = new OrderListFilter();
            Setup<IOrderViewClient, OrderListFilter, Task<BaseResponseDto<OrderListDto>>>(s => s.GetOrders(It.IsAny<OrderListFilter>())
                , f =>
                {
                    actualFilter = f;
                    return Task.FromResult(new BaseResponseDto<OrderListDto>());
                });
            var sut = Sut;
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

            await sut.GetOrdersExportForSite(currentSite, filterSpecified);

            Assert.Equal(expectedOrderListFilter, actualFilter);
        }

        [Fact]
        public async Task GetOrdersExportForSite_ShouldPassArgumentsToMicroserviceClient_WhenFilterEmpty()
        {
            var actualFilter = new OrderListFilter();
            Setup<IOrderViewClient, OrderListFilter, Task<BaseResponseDto<OrderListDto>>>(s => s.GetOrders(It.IsAny<OrderListFilter>())
                , f =>
                {
                    actualFilter = f;
                    return Task.FromResult(new BaseResponseDto<OrderListDto>());
                });
            var sut = Sut;
            var currentSite = "test_site";
            var filterEmpty = new OrderFilter { };
            var expectedOrderListFilter = new OrderListFilter
            {
                SiteName = currentSite
            };

            await sut.GetOrdersExportForSite(currentSite, filterEmpty);

            Assert.Equal(expectedOrderListFilter, actualFilter);
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

        private void SetupOrderViewClientReturning(BaseResponseDto<OrderListDto> response)
        {
            Setup<IOrderViewClient, Task<BaseResponseDto<OrderListDto>>>(ovc => ovc.GetOrders(It.IsAny<OrderListFilter>()), Task.FromResult(response));
        }
    }
}
