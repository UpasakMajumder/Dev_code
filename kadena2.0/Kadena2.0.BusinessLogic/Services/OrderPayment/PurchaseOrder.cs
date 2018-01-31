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
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;
        private readonly ISendSubmitOrder sendOrder;
        private readonly IGetOrderDataService orderDataProvider;

        public PurchaseOrder(IShoppingCartProvider shoppingCart, IKenticoResourceService resources, IKenticoDocumentProvider documents, ISendSubmitOrder sendOrder, IGetOrderDataService orderDataProvider)
        {
            if (shoppingCart == null)
            {
                throw new ArgumentNullException(nameof(shoppingCart));
            }
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }
            if (sendOrder == null)
            {
                throw new ArgumentNullException(nameof(sendOrder));
            }
            if (orderDataProvider == null)
            {
                throw new ArgumentNullException(nameof(orderDataProvider));
            }

            this.shoppingCart = shoppingCart;
            this.resources = resources;
            this.documents = documents;
            this.sendOrder = sendOrder;
            this.orderDataProvider = orderDataProvider;
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

            var redirectUrlBase = resources.GetSettingsKey("KDA_OrderSubmittedUrl");
            var redirectUrlBaseLocalized = documents.GetDocumentUrl(redirectUrlBase);
            var redirectUrl = $"{redirectUrlBaseLocalized}?success={serviceResult.Success}".ToLower();
            if (serviceResult.Success)
            {
                redirectUrl += "&order_id=" + serviceResult.Payload;
            }
            serviceResult.RedirectURL = redirectUrl;
            
            return serviceResult;
        }
    }
}
