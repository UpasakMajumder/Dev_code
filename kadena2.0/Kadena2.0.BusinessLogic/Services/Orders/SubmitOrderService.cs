using Kadena.BusinessLogic.Contracts;
using Kadena.Models.SubmitOrder;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2._0.BusinessLogic.Contracts.Orders;
using Kadena2.BusinessLogic.Contracts.OrderPayment;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class SubmitOrderService : ISubmitOrderService
    {
        private readonly IShoppingCartProvider shoppingCart;
        private readonly IGetOrderDataService orderDataProvider;
        private readonly ICreditCard3dsi creditCard3dsi;
        private readonly IPurchaseOrder purchaseOrder;

        public SubmitOrderService(IShoppingCartProvider shoppingCart, IGetOrderDataService orderDataProvider, ICreditCard3dsi creditCard3dsi, IPurchaseOrder purchaseOrder)
        {
            if (shoppingCart == null)
            {
                throw new ArgumentNullException(nameof(shoppingCart));
            }
            if (orderDataProvider == null)
            {
                throw new ArgumentNullException(nameof(orderDataProvider));
            }
            if (creditCard3dsi == null)
            {
                throw new ArgumentNullException(nameof(creditCard3dsi));
            }
            if (purchaseOrder == null)
            {
                throw new ArgumentNullException(nameof(purchaseOrder));
            }

            this.shoppingCart = shoppingCart;
            this.orderDataProvider = orderDataProvider;
            this.creditCard3dsi = creditCard3dsi;
            this.purchaseOrder = purchaseOrder;
        }

        public async Task<SubmitOrderResult> SubmitOrder(SubmitOrderRequest request)
        {
            var paymentMethods = shoppingCart.GetPaymentMethods();
            var selectedPayment = paymentMethods.FirstOrDefault(p => p.Id == (request.PaymentMethod?.Id ?? -1));
            var orderData = await orderDataProvider.GetSubmitOrderData(request);

            switch (selectedPayment?.ClassName ?? string.Empty)
            {
                case "KDA.PaymentMethods.CreditCard":
                    return creditCard3dsi.PayByCard3dsi(orderData);

                case "KDA.PaymentMethods.PurchaseOrder":
                case "KDA.PaymentMethods.MonthlyPayment":
                case "NoPaymentRequired":
                    return await purchaseOrder.SubmitPOOrder(orderData);

                case "KDA.PaymentMethods.PayPal":
                    throw new NotImplementedException("PayPal payment is not implemented yet");

                default:
                    throw new ArgumentOutOfRangeException("payment", "Unknown payment method");
            }
        }        
    }
}