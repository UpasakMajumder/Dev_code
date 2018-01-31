using Kadena.BusinessLogic.Services.Orders;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.BusinessLogic.Contracts.OrderPayment;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.WebApi
{
    public class SubmitOrderServiceTests
    {
        private PaymentMethod[] CreatePaymentMethod(string className)
        {
            return new PaymentMethod[]
                {
                    new PaymentMethod() { Id = 1, ClassName  = className  }
                };
        }

        private Kadena.Models.SubmitOrder.SubmitOrderRequest CreateOrderRequest()
        {
            return new Kadena.Models.SubmitOrder.SubmitOrderRequest
            {
                PaymentMethod = new Kadena.Models.SubmitOrder.PaymentMethod { Id = 1 }
            };
        }

        private SubmitOrderService CreateSubmitOrderService(
            string paymentMethodClassName,
            Mock<IShoppingCartProvider> shoppingCart = null,
            Mock<ICreditCard3dsi> card3dsi = null,
            Mock<ICreditCard3dsiDemo> card3dsiDemo = null, 
            Mock<IPurchaseOrder> purchaseOrder = null)
        {            
            return new SubmitOrderService(shoppingCart?.Object ?? new Mock<IShoppingCartProvider>().Object,
                card3dsi?.Object ?? new Mock<ICreditCard3dsi>().Object,
                card3dsiDemo?.Object ?? new Mock<ICreditCard3dsiDemo>().Object,
                purchaseOrder?.Object ?? new Mock<IPurchaseOrder>().Object
            );
        }

        [Theory]
        [InlineData("KDA.PaymentMethods.CreditCard", true, false, false)]
        [InlineData("KDA.PaymentMethods.CreditCardDemo", false, true, false)]
        [InlineData("KDA.PaymentMethods.PurchaseOrder", false, false, true)]
        [InlineData("KDA.PaymentMethods.MonthlyPayment", false, false, true)]
        [InlineData("NoPaymentRequired", false, false, true)]
        public async Task SubmitOrder_CallCreditCard3dsi(string paymentClass, bool call3dsi, bool call3dsiDemo, bool callPoOrder)
        {
            // Arrange
            var shoppingCart = new Mock<IShoppingCartProvider>();
            shoppingCart.Setup(s => s.GetPaymentMethods())
                .Returns(CreatePaymentMethod(paymentClass));
            var card3dsi = new Mock<ICreditCard3dsi>();
            var card3dsiDemo = new Mock<ICreditCard3dsiDemo>();
            var poOrder = new Mock<IPurchaseOrder>();
            var sut = CreateSubmitOrderService(paymentClass, shoppingCart, card3dsi, card3dsiDemo, poOrder);
            var orderRequest = CreateOrderRequest();

            // Act
            await sut.SubmitOrder(orderRequest);

            // Assert
            shoppingCart.Verify(c => c.GetPaymentMethods(), Times.Once);
            card3dsi.Verify(c => c.PayByCard3dsi(orderRequest), Times.Exactly( call3dsi ? 1 : 0 ) );
            card3dsiDemo.Verify(c => c.PayByCard3dsi(), Times.Exactly(call3dsiDemo ? 1 : 0));
            poOrder.Verify(c => c.SubmitPOOrder(orderRequest), Times.Exactly(callPoOrder ? 1 : 0));
        }


        [Theory]
        [InlineData("KDA.PaymentMethods.PayPal", false, false, false)]
        [InlineData("zsdfgsp;fbkjgoisg", false, false, false)]

        public async Task SubmitOrder_UnknownPaymentShouldRaiseException(string paymentClass, bool call3dsi, bool call3dsiDemo, bool callPoOrder)
        {
            // Arrange
            var card3dsi = new Mock<ICreditCard3dsi>();
            var card3dsiDemo = new Mock<ICreditCard3dsiDemo>();
            var poOrder = new Mock<IPurchaseOrder>();
            var sut = CreateSubmitOrderService(paymentClass, card3dsi: card3dsi, card3dsiDemo: card3dsiDemo, purchaseOrder: poOrder);
            var orderRequest = CreateOrderRequest();

            // Act
            var task = sut.SubmitOrder(orderRequest);
            var result = await Assert.ThrowsAnyAsync<Exception>( () => task );

            // Assert
            card3dsi.Verify(c => c.PayByCard3dsi(orderRequest), Times.Exactly(call3dsi ? 1 : 0));
            card3dsiDemo.Verify(c => c.PayByCard3dsi(), Times.Exactly(call3dsiDemo ? 1 : 0));
            poOrder.Verify(c => c.SubmitPOOrder(orderRequest), Times.Exactly(callPoOrder ? 1 : 0));
        }

    }
}
