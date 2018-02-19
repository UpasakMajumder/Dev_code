using Kadena.BusinessLogic.Factories;
using Kadena.Models.SubmitOrder;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.BusinessLogic.Contracts.Orders;
using Kadena2.BusinessLogic.Contracts.OrderPayment;
using System;
using System.Threading.Tasks;

namespace Kadena2.BusinessLogic.Services.OrderPayment
{
    public class PurchaseOrder : IPurchaseOrder
    {
        private readonly IShoppingCartProvider shoppingCart;
        private readonly ISendSubmitOrder sendOrder;
        private readonly IGetOrderDataService orderDataProvider;
        private readonly IOrderResultPageUrlFactory resultUrlFactory;

        public PurchaseOrder(IShoppingCartProvider shoppingCart, ISendSubmitOrder sendOrder, IGetOrderDataService orderDataProvider, IOrderResultPageUrlFactory resultUrlFactory)
        {
            if (shoppingCart == null)
            {
                throw new ArgumentNullException(nameof(shoppingCart));
            }
            if (sendOrder == null)
            {
                throw new ArgumentNullException(nameof(sendOrder));
            }
            if (orderDataProvider == null)
            {
                throw new ArgumentNullException(nameof(orderDataProvider));
            }
            if (resultUrlFactory == null)
            {
                throw new ArgumentNullException(nameof(resultUrlFactory));
            }

            this.shoppingCart = shoppingCart;
            this.sendOrder = sendOrder;
            this.orderDataProvider = orderDataProvider;
            this.resultUrlFactory = resultUrlFactory;
        }

        public async Task<SubmitOrderResult> SubmitPOOrder(SubmitOrderRequest request)
        {
            var orderData = await orderDataProvider.GetSubmitOrderData(request);
            var serviceResult = await sendOrder.SubmitOrderData(orderData);

            if (serviceResult.Success)
            {
                shoppingCart.RemoveCurrentItemsFromStock();
                shoppingCart.ClearCart();
            }

            serviceResult.RedirectURL = resultUrlFactory.GetOrderResultPageUrl(serviceResult.Success, serviceResult.Payload);

            return serviceResult;
        }
    }
}
