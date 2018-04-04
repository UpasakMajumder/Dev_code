﻿using Kadena.BusinessLogic.Services.Orders;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena.Dto.Order.Failed;
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
    public class OrderResubmissionServiceTests
    {
        [Fact]
        public void ServiceConstructor_ShouldThrow_WhenArgumentsNull()
        {
            Action action = () => new OrderResubmissionService(null, null, null, null);

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public async Task ResubmitOrder_ShouldCallClient_WithGivenOrderId()
        {
            var actualArgs = new ResubmitOrderRequestDto();
            var resubmitClient = new Mock<IOrderResubmitClient>();
            resubmitClient
                .Setup(rc => rc.Resubmit(It.IsAny<ResubmitOrderRequestDto>()))
                .Returns<ResubmitOrderRequestDto>((req) => 
                {
                    actualArgs = req;
                    return Task.FromResult(new BaseResponseDto<ResubmitOrderResponseDto>());
                });


            var sut = new OrderResubmissionServiceBuilder()
                .WithOrderResubmitClient(resubmitClient.Object)
                .Build();
            var orderId = "123";

            await sut.ResubmitOrder(orderId);

            Assert.Equal(orderId, actualArgs.OrderId);
        }

        [Fact]
        public async Task ResubmitOrder_ShouldLogResponse()
        {
            var logger = new Mock<IKenticoLogger>();

            var sut = new OrderResubmissionServiceBuilder()
                .WithLogger(logger.Object)
                .Build();

            await sut.ResubmitOrder("123");

            logger.Verify(log => log.LogInfo(
                nameof(OrderResubmissionService),
                nameof(OrderResubmissionService.ResubmitOrder),
                It.IsAny<string>()
                ));
        }

        [Fact]
        public async Task ResubmitOrder_ShouldReturnFailure_WhenCallFails()
        {
            var failureResult = Task.FromResult(new BaseResponseDto<ResubmitOrderResponseDto>
            {
                Success = false,
                Error = new BaseErrorDto { Message = "oh noes" },
            });
            var resubmitClient = new Mock<IOrderResubmitClient>();
            resubmitClient
                .Setup(rc => rc.Resubmit(It.IsAny<ResubmitOrderRequestDto>()))
                .Returns(failureResult);
            var sut = new OrderResubmissionServiceBuilder()
                .WithOrderResubmitClient(resubmitClient.Object)
                .Build();

            var expected = new OperationResult
            {
                Success = false,
                ErrorMessage = "oh noes"
            };

            var result = await sut.ResubmitOrder("123");

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetFailedOrders_ShouldThrow_WhenPageNumberInvalid()
        {
            var invalidPageNumber = 0;
            var validNumberOfItemsPerPage = 100;
            var sut = new OrderResubmissionServiceBuilder()
                .Build();

            Func<Task> action = () => sut.GetFailedOrders(invalidPageNumber, validNumberOfItemsPerPage);

            await Assert.ThrowsAsync<ArgumentException>("page", action);
        }

        [Fact]
        public async Task GetFailedOrders_ShouldThrow_WhenItemsPerPageInvalid()
        {
            var validPageNumber = 1;
            var invalidNumberOfItemsPerPage = 0;
            var sut = new OrderResubmissionServiceBuilder()
                .Build();

            Func<Task> action = () => sut.GetFailedOrders(validPageNumber, invalidNumberOfItemsPerPage);

            await Assert.ThrowsAsync<ArgumentException>("itemsPerPage", action);
        }

        [Fact]
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

            var orderClient = new Mock<IOrderViewClient>();

            var sut = new OrderResubmissionServiceBuilder()
                .WithOrderViewClient(orderClient.Object)
                .Build();

            await sut.GetFailedOrders(page, itemsPerPage);

            orderClient.Verify(oc => oc.GetOrders(expectedFilter), Times.Once());
        }

        [Fact]
        public async Task GetFailedOrders_ShouldMapDataFromMicroserviceResponse()
        {
            var orderStatusMapped = "order_status_mapped";
            var expectedResult = new PagedData<FailedOrder>
            {
                Data = new List<FailedOrder>
                {
                    new FailedOrder
                    {
                        Id = "order_id",
                        OrderDate = new DateTime(2018, 3, 21),
                        OrderStatus = orderStatusMapped,
                        SiteName = "order_site",
                        SubmissionAttemptsCount = 2,
                        TotalPrice = 3.5m
                    }
                }
            };
            var orderStatus = "order_status";
            var orders = new OrderListDto
            {
                Orders = new[]
                {
                    new RecentOrderDto
                    {
                        Id = "order_id",
                        CreateDate = new DateTime(2018, 3, 21),
                        Status = orderStatus,
                        SiteName = "order_site",
                        StatusAmounts = new Dictionary<int, int>
                        {
                            { (int)OrderStatus.SentToTibcoError, 2 }
                        },
                        TotalPrice = 3.5m
                    }
                }
            };

            var orderClient = new Mock<IOrderViewClient>();
            orderClient
                .Setup(oc => oc.GetOrders(It.IsAny<OrderListFilter>()))
                .Returns(Task.FromResult(new BaseResponseDto<OrderListDto> { Success = true, Payload = orders }));

            var orderProvider = new Mock<IKenticoOrderProvider>();
            orderProvider
                .Setup(kop => kop.MapOrderStatus(orderStatus))
                .Returns(orderStatusMapped);

            var sut = new OrderResubmissionServiceBuilder()
                .WithOrderViewClient(orderClient.Object)
                .WithOrderProvider(orderProvider.Object)
                .Build();

            var result = await sut.GetFailedOrders(1, 1);

            Assert.True(expectedResult.Data.Count == result.Data.Count);
            Assert.True(AreEqual(expectedResult.Data[0], result.Data[0]));
        }

        [Fact]
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
            orderClient
                .Setup(oc => oc.GetOrders(It.IsAny<OrderListFilter>()))
                .Returns(Task.FromResult(new BaseResponseDto<OrderListDto> { Success = true, Payload = orders }));

            var sut = new OrderResubmissionServiceBuilder()
                .WithOrderViewClient(orderClient.Object)
                .Build();

            var result = await sut.GetFailedOrders(page, itemsPerPage);

            Assert.Equal(expectedResult.Pagination, result.Pagination);
        }

        [Fact]
        public async Task GetFailedOrders_ShouldReturnEmptyResult_WhenMicroserviceFails()
        {
            var itemsPerPage = 2;
            var page = 2;
            var expectedResult = PagedData<FailedOrder>.Empty();

            var orderClient = new Mock<IOrderViewClient>();
            orderClient
                .Setup(oc => oc.GetOrders(It.IsAny<OrderListFilter>()))
                .Returns(Task.FromResult(new BaseResponseDto<OrderListDto> { Success = false }));

            var sut = new OrderResubmissionServiceBuilder()
                .WithOrderViewClient(orderClient.Object)
                .Build();

            var result = await sut.GetFailedOrders(page, itemsPerPage);

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

        private class OrderResubmissionServiceBuilder
        {
            IOrderResubmitClient orderResubmitClient = Mock.Of<IOrderResubmitClient>();
            IOrderViewClient orderViewClient = Mock.Of<IOrderViewClient>();
            IKenticoOrderProvider kenticoOrderProvider = Mock.Of<IKenticoOrderProvider>();
            IKenticoLogger kenticoLogger = Mock.Of<IKenticoLogger>();

            public OrderResubmissionServiceBuilder WithOrderResubmitClient(IOrderResubmitClient client)
            {
                orderResubmitClient = client;
                return this;
            }

            public OrderResubmissionServiceBuilder WithOrderViewClient(IOrderViewClient client)
            {
                orderViewClient = client;
                return this;
            }

            public OrderResubmissionServiceBuilder WithOrderProvider(IKenticoOrderProvider provider)
            {
                kenticoOrderProvider = provider;
                return this;
            }

            public OrderResubmissionServiceBuilder WithLogger(IKenticoLogger logger)
            {
                kenticoLogger = logger;
                return this;
            }

            public OrderResubmissionService Build()
                => new OrderResubmissionService(
                    orderResubmitClient,
                    orderViewClient,
                    kenticoOrderProvider,
                    kenticoLogger
                );
        }
    }
}
