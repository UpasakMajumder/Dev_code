using Kadena2.BusinessLogic.Services.OrderPayment;
using Xunit;
using Kadena.Models.SubmitOrder;
using System.Threading.Tasks;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using Kadena.BusinessLogic.Factories;
using Kadena2.BusinessLogic.Contracts.Orders;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;

namespace Kadena.Tests.CreditCard
{
    public class SavedCreditCard3dsiTests : KadenaUnitTest<SavedCreditCard3dsi>
    {
        [Fact(DisplayName = "SavedCreditCard3dsi.PayBySavedCard3dsi() | Failed")]
        public async Task SavedCard3dsi_MissingCardId()
        {
            // Act
            var result = await Sut.PayBySavedCard3dsi(new SubmitOrderRequest());

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Verify<IKenticoLogger>(l => l.LogError("PayOrderBySavedCard", "No saved card Id was given"), Times.Once);
            Verify<IOrderResultPageUrlFactory>(f => f.GetCardPaymentResultPageUrl(false, "", "", "Kadena.OrderByCardFailed.ApprovalFailed"), Times.Once);
            Verify<IOrderResultPageUrlFactory>(f => f.GetOrderResultPageUrl(It.IsAny<SubmitOrderResult>()), Times.Never);
        }

        [Fact(DisplayName = "SavedCreditCard3dsi.PayBySavedCard3dsi() | Success")]
        public async Task SavedCard3dsi_Payed()
        {
            // Arrange
            const string cardTokenId = "storedCardId123";
            const string newOrderId = "orderid123";
            var request = new SubmitOrderRequest() { PaymentMethod = new PaymentMethod { Card = cardTokenId } };
            var orderDto = new OrderDTO { PaymentOption = new PaymentOptionDTO { TokenId = cardTokenId } };

            Setup<IGetOrderDataService, Task<OrderDTO>>(o => o.GetSubmitOrderData(request), Task.FromResult(orderDto));
            Setup<ISendSubmitOrder, Task<SubmitOrderResult>>(s => s.SubmitOrderData(orderDto), Task.FromResult(new SubmitOrderResult { Success = true, OrderId = newOrderId }));

            // Act
            var result = await Sut.PayBySavedCard3dsi(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Verify<IKenticoLogger>(l => l.LogInfo(It.IsAny<string>(), It.IsAny<string>(), It.Is<string>(s => s.Contains(cardTokenId))), Times.AtLeastOnce);
            Verify<IOrderResultPageUrlFactory>(f => f.GetCardPaymentResultPageUrl(true, newOrderId, "", ""), Times.Once);
            Verify<IOrderResultPageUrlFactory>(f => f.GetOrderResultPageUrl(It.IsAny<SubmitOrderResult>()), Times.Never);
            Verify<IShoppingCartProvider>(s => s.ClearCart(0), Times.Once);
            Verify<IShoppingCartProvider>(s => s.UpdateInventory(0), Times.Once);
        }
    }
}
