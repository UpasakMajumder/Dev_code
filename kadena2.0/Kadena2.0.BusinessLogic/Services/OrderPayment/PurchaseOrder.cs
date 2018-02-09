using Kadena.BusinessLogic.Factories;
using Kadena.Models.SubmitOrder;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2._0.BusinessLogic.Contracts.Orders;
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
        private readonly IKenticoResourceService resources;

        public PurchaseOrder(IShoppingCartProvider shoppingCart, ISendSubmitOrder sendOrder, IGetOrderDataService orderDataProvider, IOrderResultPageUrlFactory resultUrlFactory, IKenticoResourceService resources)
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
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }

            this.shoppingCart = shoppingCart;
            this.sendOrder = sendOrder;
            this.orderDataProvider = orderDataProvider;
            this.resultUrlFactory = resultUrlFactory;
            this.resources = resources;
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

            var resultPagePath = resources.GetSettingsKey("KDA_OrderSubmittedUrl");
            var redirectUrl = resultUrlFactory.GetOrderResultPageUrl(resultPagePath, serviceResult.Success, serviceResult.Payload);

            serviceResult.RedirectURL = redirectUrl;
            
            return serviceResult;
        }
    }
}
