using Kadena.BusinessLogic.Contracts;
using Kadena.Models.SubmitOrder;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.BusinessLogic.Contracts.OrderPayment;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class SubmitOrderService : ISubmitOrderService
    {
        private readonly IShoppingCartProvider shoppingCart;
        private readonly ICreditCard3dsi creditCard3dsi;
        private readonly ISavedCreditCard3dsi savedcreditCard3dsi;
        private readonly ICreditCard3dsiDemo creditCard3dsiDemo;
        private readonly IPurchaseOrder purchaseOrder;

        public SubmitOrderService(IShoppingCartProvider shoppingCart, ICreditCard3dsi creditCard3dsi, ISavedCreditCard3dsi savedcreditCard3dsi, ICreditCard3dsiDemo creditCard3dsiDemo, IPurchaseOrder purchaseOrder)
        {
            this.shoppingCart = shoppingCart ?? throw new ArgumentNullException(nameof(shoppingCart));
            this.creditCard3dsi = creditCard3dsi ?? throw new ArgumentNullException(nameof(creditCard3dsi));
            this.savedcreditCard3dsi = savedcreditCard3dsi ?? throw new ArgumentNullException(nameof(savedcreditCard3dsi));
            this.creditCard3dsiDemo = creditCard3dsiDemo ?? throw new ArgumentNullException(nameof(creditCard3dsiDemo));
            this.purchaseOrder = purchaseOrder ?? throw new ArgumentNullException(nameof(purchaseOrder));
        }

        public async Task<SubmitOrderResult> SubmitOrder(SubmitOrderRequest request)
        {
            var paymentMethods = shoppingCart.GetPaymentMethods();
            var selectedPayment = paymentMethods.FirstOrDefault(p => p.Id == (request.PaymentMethod?.Id ?? -1));

            switch (selectedPayment?.ClassName ?? string.Empty)
            {
                case "KDA.PaymentMethods.CreditCard":
                    return await PayByCard(request);

                case "KDA.PaymentMethods.CreditCardDemo":
                    return creditCard3dsiDemo.PayByCard3dsi();

                case "KDA.PaymentMethods.PurchaseOrder":
                case "KDA.PaymentMethods.MonthlyPayment":
                case "NoPaymentRequired":
                    return await purchaseOrder.SubmitPOOrder(request);

                case "KDA.PaymentMethods.PayPal":
                    throw new NotImplementedException("PayPal payment is not implemented yet");

                default:
                    throw new ArgumentOutOfRangeException("payment", "Unknown payment method");
            }
        }

        private async Task<SubmitOrderResult> PayByCard(SubmitOrderRequest request)
        {
            var savedCardId = request.PaymentMethod.Card;

            if (string.IsNullOrEmpty(savedCardId))
            {
                return await creditCard3dsi.PayByCard3dsi(request);
            }
            else
            {
                return await savedcreditCard3dsi.PayBySavedCard3dsi(request);
            }
        }
    }
}