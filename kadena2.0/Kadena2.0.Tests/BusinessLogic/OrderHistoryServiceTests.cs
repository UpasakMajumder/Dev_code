using Kadena.BusinessLogic.Services.Orders;
using Kadena.Dto.General;
using Kadena.Dto.ViewOrder.MicroserviceResponses;
using Kadena.Dto.ViewOrder.MicroserviceResponses.History;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class OrderHistoryServiceTests : KadenaUnitTest<OrderHistoryService>
    {
        [Theory]
        [ClassData(typeof(OrderHistoryServiceTests))]
        public void ServiceConstructor_ShouldThrow_WhenAnyArgumentsNull(
            IKenticoResourceService kenticoResourceService,
            IOrderManualUpdateClient orderHistoryClient,
            IOrderViewClient orderViewClient,
            IKenticoUserProvider kenticoUserProvider,
            IKenticoOrderProvider kenticoOrderProvider)
        {
            Assert.Throws<ArgumentNullException>(() => new OrderHistoryService(
                kenticoResourceService,
                orderHistoryClient,
                orderViewClient,
                kenticoUserProvider,
                kenticoOrderProvider)
            );
        }

        [Fact]
        public async Task GetOrderHistory_ShouldGetApprovalMessages()
        {
            var orderId = "123";
            SetupEmptyHistory(orderId);
            Setup<IOrderViewClient, Task<BaseResponseDto<GetOrderByOrderIdResponseDTO>>>(
                ovc => ovc.GetOrderByOrderId(orderId),
                Task.FromResult(new BaseResponseDto<GetOrderByOrderIdResponseDTO>
                {
                    Success = true,
                    Payload = new GetOrderByOrderIdResponseDTO
                    {
                        Approvals = new List<ApprovalDTO>
                        {
                            new ApprovalDTO { Note = "This" },
                            new ApprovalDTO { Note = "is" },
                            new ApprovalDTO { Note = "Sparta" },
                        }
                    }
                }));

            var history = await Sut.GetOrderHistory(orderId);

            var expectedMessage = "This, is, Sparta";
            Assert.Equal(expectedMessage, history.Message.Text);
        }

        [Fact]
        public async Task GetOrderHistory_ShouldDetectStatusChange()
        {
            const string orderId = "123";
            const int statusChangeRecordTypeId = 2;
            SetupOrder(orderId);
            Setup<IOrderManualUpdateClient, Task<BaseResponseDto<List<OrderHistoryUpdateDto>>>>(
                ohc => ohc.Get(orderId),
                Task.FromResult(new BaseResponseDto<List<OrderHistoryUpdateDto>>
                {
                    Success = true,
                    Payload = new List<OrderHistoryUpdateDto>
                    {
                        new OrderHistoryUpdateDto { Status = "Initial" },
                        new OrderHistoryUpdateDto { Status = "Something status related", RecordTypeId = statusChangeRecordTypeId },
                    }
                }
                ));

            var history = await Sut.GetOrderHistory(orderId);

            Assert.True(history.OrderChanges.Items.Count == 1);
        }

        [Theory]
        [InlineData(8, "8")] // item quantity change
        [InlineData(0, "")] // item removed
        public async Task GetOrderHistory_ShouldDetectItemQuantityChange(int newQuantity, string detectedNewQuantity)
        {
            const string orderId = "123";
            const int statusChangeRecordTypeId = 5;
            SetupOrder(orderId);
            Setup<IOrderManualUpdateClient, Task<BaseResponseDto<List<OrderHistoryUpdateDto>>>>(
                ohc => ohc.Get(orderId),
                Task.FromResult(new BaseResponseDto<List<OrderHistoryUpdateDto>>
                {
                    Success = true,
                    Payload = new List<OrderHistoryUpdateDto>
                    {
                        new OrderHistoryUpdateDto
                        {
                            Status = "Initial",
                            Items = new [] 
                            {
                                new ItemDto { Quantity = 2, LineNumber = "1" },
                                new ItemDto { Quantity = 4, LineNumber = "2" },
                            }
                        },
                        new OrderHistoryUpdateDto
                        {
                            Status = "Data update",
                            RecordTypeId = statusChangeRecordTypeId,
                            Items = new[]
                            {
                                new ItemDto { Quantity = 2, LineNumber = "1" },
                                new ItemDto { Quantity = newQuantity, LineNumber = "2" },
                            }
                        },
                    }
                }
                ));

            var history = await Sut.GetOrderHistory(orderId);

            Assert.Equal(detectedNewQuantity, history.ItemChanges.Items.First().NewValue);
        }

        private void SetupEmptyHistory(string orderId)
        {
            Setup<IOrderManualUpdateClient, Task<BaseResponseDto<List<OrderHistoryUpdateDto>>>>(
                ohc => ohc.Get(orderId),
                Task.FromResult(new BaseResponseDto<List<OrderHistoryUpdateDto>>
                {
                    Success = true,
                    Payload = new List<OrderHistoryUpdateDto>()
                }
                ));
        }

        private void SetupOrder(string orderId)
        {
            Setup<IOrderViewClient, Task<BaseResponseDto<GetOrderByOrderIdResponseDTO>>>(
                ovc => ovc.GetOrderByOrderId(orderId),
                Task.FromResult(new BaseResponseDto<GetOrderByOrderIdResponseDTO>
                {
                    Success = true,
                    Payload = new GetOrderByOrderIdResponseDTO()
                }));
        }
    }
}
