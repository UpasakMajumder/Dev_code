using Kadena.BusinessLogic.Services.Orders;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.BusinessLogic.Contracts.OrderPayment;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class SubmitOrderServiceTests : KadenaUnitTest<SubmitOrderService>
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

        [Theory(DisplayName = "SubmitOrderService.SubmitOrder() | Request to 3DSi")]
        [InlineData("KDA.PaymentMethods.CreditCard", true, false, false)]
        [InlineData("KDA.PaymentMethods.CreditCardDemo", false, true, false)]
        [InlineData("KDA.PaymentMethods.PurchaseOrder", false, false, true)]
        [InlineData("KDA.PaymentMethods.MonthlyPayment", false, false, true)]
        [InlineData("NoPaymentRequired", false, false, true)]
        public async Task SubmitOrder_CallCreditCard3dsi(string paymentClass, bool call3dsi, bool call3dsiDemo, bool callPoOrder)
        {
            // Arrange
            Setup<IShoppingCartProvider, PaymentMethod[]>(s => s.GetPaymentMethods(), CreatePaymentMethod(paymentClass));
            var orderRequest = CreateOrderRequest();

            // Act
            await Sut.SubmitOrder(orderRequest);

            // Assert
            Verify<IShoppingCartProvider>(c => c.GetPaymentMethods(), Times.Once);
            Verify<ICreditCard3dsi>(c => c.PayByCard3dsi(orderRequest), Times.Exactly( call3dsi ? 1 : 0 ) );
            Verify<ICreditCard3dsiDemo>(c => c.PayByCard3dsi(), Times.Exactly(call3dsiDemo ? 1 : 0));
            Verify<IPurchaseOrder>(c => c.SubmitPOOrder(orderRequest), Times.Exactly(callPoOrder ? 1 : 0));
        }

        [Theory(DisplayName = "SubmitOrderService.SubmitOrder() | Request to 3DSi using saved card")]
        [InlineData("someCardIdxxxxxxxxxxxx", false, true)]
        [InlineData("", true, false)]
        public async Task SubmitOrder_CallSavedCreditCard3dsi( string savedCardid, bool call3dsi, bool callSaved3dsi)
        {
            // Arrange
            Setup<IShoppingCartProvider, PaymentMethod[]>(s => s.GetPaymentMethods(), CreatePaymentMethod("KDA.PaymentMethods.CreditCard"));

            var orderRequest = CreateOrderRequest();
            orderRequest.PaymentMethod.Card = savedCardid;

            // Act
            await Sut.SubmitOrder(orderRequest);

            // Assert
            Verify<ICreditCard3dsi>(c => c.PayByCard3dsi(orderRequest), Times.Exactly(call3dsi ? 1 : 0));
            Verify<ISavedCreditCard3dsi>(c => c.PayBySavedCard3dsi(orderRequest), Times.Exactly(callSaved3dsi ? 1 : 0));
        }


        [Theory(DisplayName = "SubmitOrderService.SubmitOrder() | Unknown payment")]
        [InlineData("KDA.PaymentMethods.PayPal", false, false, false)]
        [InlineData("zsdfgsp;fbkjgoisg", false, false, false)]
        public async Task SubmitOrder_UnknownPaymentShouldRaiseException(string paymentClass, bool call3dsi, bool call3dsiDemo, bool callPoOrder)
        {
            // Arrange
            Setup<IShoppingCartProvider, PaymentMethod[]>(c => c.GetPaymentMethods(), CreatePaymentMethod(paymentClass));
            var orderRequest = CreateOrderRequest();

            // Act
            Task action() => Sut.SubmitOrder(orderRequest);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(action);
            Verify<ICreditCard3dsi>(c => c.PayByCard3dsi(orderRequest), Times.Exactly(call3dsi ? 1 : 0));
            Verify<ICreditCard3dsiDemo>(c => c.PayByCard3dsi(), Times.Exactly(call3dsiDemo ? 1 : 0));
            Verify<IPurchaseOrder>(c => c.SubmitPOOrder(orderRequest), Times.Exactly(callPoOrder ? 1 : 0));
        }

    }
}
