using Kadena2.BusinessLogic.Services.OrderPayment;
using Moq.AutoMock;
using Xunit;
using Kadena.Models.SubmitOrder;
using System.Threading.Tasks;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using Kadena.BusinessLogic.Factories;
using Kadena2.BusinessLogic.Contracts.Orders;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;

namespace Kadena.Tests._3dsiCreditCard
{
    public class SavedCreditCard3dsiTests
    {
        [Fact]
        public async Task SavedCard3dsi_MissingCardId()
        {
            // Arrange
            var autoMocker = new AutoMocker();
            var sut = autoMocker.CreateInstance<SavedCreditCard3dsi>();
            var request = new SubmitOrderRequest();
            var resultfactoryMock = autoMocker.GetMock<IOrderResultPageUrlFactory>();

            // Act
            var result = await sut.PayBySavedCard3dsi(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            autoMocker.GetMock<IKenticoLogger>().Verify(l => l.LogError("PayOrderBySavedCard", "No saved card Id was given"), Times.Once);
            resultfactoryMock.Verify(f => f.GetCardPaymentResultPageUrl(false, "", ""), Times.Once);
            resultfactoryMock.Verify(f => f.GetOrderResultPageUrl(It.IsAny<bool>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task SavedCard3dsi_Payed()
        {
            // Arrange
            const string cardTokenId = "storedCardId123";
            const string newOrderId = "orderid123";
            var autoMocker = new AutoMocker();
            var sut = autoMocker.CreateInstance<SavedCreditCard3dsi>();
            var request = new SubmitOrderRequest() { PaymentMethod = new PaymentMethod { Card = cardTokenId } };
            var resultfactoryMock = autoMocker.GetMock<IOrderResultPageUrlFactory>();
            var orderDataProviderMock = autoMocker.GetMock<IGetOrderDataService>();
            var orderDto = new OrderDTO { PaymentOption = new PaymentOptionDTO { TokenId = cardTokenId } };
            orderDataProviderMock.Setup(o => o.GetSubmitOrderData(request))
                .Returns(Task.FromResult(orderDto));
            var saveOrderMock = autoMocker.GetMock<ISendSubmitOrder>();
            saveOrderMock.Setup(s => s.SubmitOrderData(orderDto))
                .Returns(Task.FromResult(new SubmitOrderResult { Success = true, Payload = newOrderId }));
            var shoppingCartMock = autoMocker.GetMock<IShoppingCartProvider>();

            // Act
            var result = await sut.PayBySavedCard3dsi(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            autoMocker.GetMock<IKenticoLogger>().Verify(l => l.LogInfo(It.IsAny<string>(), It.IsAny<string>(), It.Is<string>(s => s.Contains(cardTokenId))), Times.AtLeastOnce);
            resultfactoryMock.Verify(f => f.GetCardPaymentResultPageUrl(true, newOrderId, ""), Times.Once);
            resultfactoryMock.Verify(f => f.GetOrderResultPageUrl(It.IsAny<bool>(), It.IsAny<string>()), Times.Never);
            shoppingCartMock.Verify(s => s.ClearCart(0), Times.Once);
            shoppingCartMock.Verify(s => s.RemoveCurrentItemsFromStock(0), Times.Once);
        }
    }
}
