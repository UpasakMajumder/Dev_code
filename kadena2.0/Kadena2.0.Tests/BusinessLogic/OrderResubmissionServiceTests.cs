using Kadena.BusinessLogic.Services.Orders;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena.Dto.Order.Failed;
using Kadena.Models;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using Kadena.Models.Orders.Failed;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.OrderResubmission
{
    public class OrderResubmissionServiceTests : KadenaUnitTest<OrderResubmissionService>
    {
        [Fact(DisplayName = "OrderResubmissionService()")]
        public void ServiceConstructor_ShouldThrow_WhenArgumentsNull()
        {
            Use<IOrderResubmitClient>(null);
            Use<IOrderViewClient>(null);
            Use<IKenticoOrderProvider>(null);
            Use<IKenticoLogger>(null);

            Action action = () => { var sut = Sut; };

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact(DisplayName = "OrderResubmissionService.ResubmitOrder() | Microservice call")]
        public async Task ResubmitOrder_ShouldCallClient_WithGivenOrderId()
        {
            var actualArgs = new ResubmitOrderRequestDto();
            Setup<IOrderResubmitClient, ResubmitOrderRequestDto, Task<BaseResponseDto<ResubmitOrderResponseDto>>>(rc => rc.Resubmit(It.IsAny<ResubmitOrderRequestDto>())
                , (req) =>
                {
                    actualArgs = req;
                    return Task.FromResult(new BaseResponseDto<ResubmitOrderResponseDto>());
                });
            var orderId = "123";

            await Sut.ResubmitOrder(orderId);

            Assert.Equal(orderId, actualArgs.OrderId);
        }

        [Fact(DisplayName = "OrderResubmissionService.ResubmitOrder() | Logger call")]
        public async Task ResubmitOrder_ShouldLogResponse()
        {
            await Sut.ResubmitOrder("123");

            Verify<IKenticoLogger>(log => log.LogInfo(
                nameof(OrderResubmissionService),
                nameof(OrderResubmissionService.ResubmitOrder),
                It.IsAny<string>()
                )
                , Times.Once);
        }

        [Fact(DisplayName = "OrderResubmissionService.ResubmitOrder() | Failed")]
        public async Task ResubmitOrder_ShouldReturnFailure_WhenCallFails()
        {
            var failureResult = Task.FromResult(new BaseResponseDto<ResubmitOrderResponseDto>
            {
                Success = false,
                Error = new BaseErrorDto { Message = "oh noes" },
            });
            Setup<IOrderResubmitClient, Task<BaseResponseDto<ResubmitOrderResponseDto>>>(rc => rc.Resubmit(It.IsAny<ResubmitOrderRequestDto>()), failureResult);
            var expected = new OperationResult
            {
                Success = false,
                ErrorMessage = "oh noes"
            };

            var result = await Sut.ResubmitOrder("123");

            Assert.Equal(expected, result);
        }

        [Fact(DisplayName = "OrderResubmissionService.GetFailedOrders() | Invalid page number")]
        public async Task GetFailedOrders_ShouldThrow_WhenPageNumberInvalid()
        {
            var invalidPageNumber = 0;
            var validNumberOfItemsPerPage = 100;

            Task action() => Sut.GetFailedOrders(invalidPageNumber, validNumberOfItemsPerPage);

            await Assert.ThrowsAsync<ArgumentException>("page", action);
        }

        [Fact(DisplayName = "OrderResubmissionService.GetFailedOrders() | Invalid items per page")]
        public async Task GetFailedOrders_ShouldThrow_WhenItemsPerPageInvalid()
        {
            var validPageNumber = 1;
            var invalidNumberOfItemsPerPage = 0;


            Task action() => Sut.GetFailedOrders(validPageNumber, invalidNumberOfItemsPerPage);

            await Assert.ThrowsAsync<ArgumentException>("itemsPerPage", action);
        }

        [Fact(DisplayName = "OrderResubmissionService.GetFailedOrders() | Microservice call")]
        public async Task GetFailedOrders_ShouldPassArgumentsToMicroserviceClient()
        {
            var page = 5;
            var itemsPerPage = 12;
            var status = (int)OrderStatus.SentToTibcoError;
            var expectedFilter = new OrderListFilter
            {
                PageNumber = page,
                ItemsPerPage = itemsPerPage,
                StatusHistoryContains = status
            };

            await Sut.GetFailedOrders(page, itemsPerPage);

            Verify<IOrderViewClient>(oc => oc.GetOrders(expectedFilter), Times.Once());
        }

        [Fact(DisplayName = "OrderResubmissionService.GetFailedOrders() | Microservice call")]
        public async Task GetFailedOrders_ShouldMapDataFromMicroserviceResponse()
        {
            const string customerEmail = "just@in.time";
            var expectedResult = new PagedData<FailedOrder>
            {
                Data = new List<FailedOrder>
                {
                    new FailedOrder
                    {
                        Id = "order_id",
                        OrderDate = new DateTime(2018, 3, 21),
                        OrderStatus = "order_status",
                        CustomerName = customerEmail,
                        SiteName = "order_site",
                        SubmissionAttemptsCount = 2,
                        TotalPrice = 3.5m
                    }
                }
            };
            var orders = new OrderListDto
            {
                Orders = new[]
                {
                    new RecentOrderDto
                    {
                        Id = "order_id",
                        CreateDate = new DateTime(2018, 3, 21),
                        Status = "order_status",
                        SiteName = "order_site",
                        StatusAmounts = new Dictionary<int, int>
                        {
                            { (int)OrderStatus.SentToTibcoError, 2 }
                        },
                        TotalPrice = 3.5m
                    }
                }
            };
            Setup<IOrderViewClient, Task<BaseResponseDto<OrderListDto>>>(oc => oc.GetOrders(It.IsAny<OrderListFilter>())
                , Task.FromResult(new BaseResponseDto<OrderListDto> { Success = true, Payload = orders }));
            Setup<IKenticoCustomerProvider, Customer>(cp => cp.GetCustomer(It.IsAny<int>()), new Customer { Email = customerEmail });

            var result = await Sut.GetFailedOrders(1, 1);

            Assert.True(expectedResult.Data.Count == result.Data.Count);
            Assert.True(AreEqual(expectedResult.Data[0], result.Data[0]));
        }

        [Fact(DisplayName = "OrderResubmissionService.GetFailedOrders() | Pagination mapping")]
        public async Task GetFailedOrders_ShouldMapPaginationFromMicroserviceResponse()
        {
            var totalItems = 7;
            var itemsPerPage = 2;
            var page = 2;
            var pageCount = 4;
            var expectedResult = new PagedData<FailedOrder>
            {
                Pagination = new Pagination
                {
                    CurrentPage = page,
                    PagesCount = pageCount,
                    RowsCount = totalItems,
                    RowsOnPage = itemsPerPage
                }
            };

            var orders = new OrderListDto
            {
                TotalCount = totalItems,
                Orders = new List<RecentOrderDto>()
            };

            var orderClient = new Mock<IOrderViewClient>();
            Setup<IOrderViewClient, Task<BaseResponseDto<OrderListDto>>>(oc => oc.GetOrders(It.IsAny<OrderListFilter>())
                , Task.FromResult(new BaseResponseDto<OrderListDto> { Success = true, Payload = orders }));

            var result = await Sut.GetFailedOrders(page, itemsPerPage);

            Assert.Equal(expectedResult.Pagination, result.Pagination);
        }

        [Fact(DisplayName = "OrderResubmissionService.GetFailedOrders() | Microservice call failed")]
        public async Task GetFailedOrders_ShouldReturnEmptyResult_WhenMicroserviceFails()
        {
            var itemsPerPage = 2;
            var page = 2;
            var expectedResult = PagedData<FailedOrder>.Empty();

            var orderClient = new Mock<IOrderViewClient>();
            Setup<IOrderViewClient, Task<BaseResponseDto<OrderListDto>>>(oc => oc.GetOrders(It.IsAny<OrderListFilter>())
                , Task.FromResult(new BaseResponseDto<OrderListDto> { Success = false }));

            var result = await Sut.GetFailedOrders(page, itemsPerPage);

            Assert.Equal(expectedResult.Data.Count, result.Data.Count);
            Assert.Equal(expectedResult.Pagination, result.Pagination);
        }

        private bool AreEqual(FailedOrder expected, FailedOrder actual)
            => expected.Id == actual.Id
                && expected.OrderDate == actual.OrderDate
                && expected.OrderStatus == actual.OrderStatus
                && expected.SiteName == actual.SiteName
                && expected.SubmissionAttemptsCount == actual.SubmissionAttemptsCount
                && expected.TotalPrice == actual.TotalPrice;
    }
}
