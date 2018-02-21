using Kadena.Models.SubmitOrder;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.BusinessLogic.Contracts.Orders;
using Kadena2.BusinessLogic.Contracts.OrderPayment;
using System;
using System.Threading.Tasks;
using Kadena.BusinessLogic.Factories;

namespace Kadena2.BusinessLogic.Services.OrderPayment
{
    public class SavedCreditCard3dsi : ISavedCreditCard3dsi
    {
        private readonly IKenticoLogger logger;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;
        private readonly ISendSubmitOrder sendOrder;
        private readonly IShoppingCartProvider shoppingCart;
        private readonly IGetOrderDataService orderDataProvider;
        private readonly IOrderResultPageUrlFactory resultUrlFactory;

        public SavedCreditCard3dsi(IKenticoLogger logger, 
                                   IKenticoResourceService resources, 
                                   IKenticoDocumentProvider documents, 
                                   ISendSubmitOrder sendOrder, 
                                   IShoppingCartProvider shoppingCart, 
                                   IGetOrderDataService orderDataProvider, 
                                   IOrderResultPageUrlFactory resultUrlFactory)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
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
            if (shoppingCart == null)
            {
                throw new ArgumentNullException(nameof(shoppingCart));
            }
            if (orderDataProvider == null)
            {
                throw new ArgumentNullException(nameof(orderDataProvider));
            }
            if (resultUrlFactory == null)
            {
                throw new ArgumentNullException(nameof(resultUrlFactory));
            }

            this.logger = logger;
            this.resources = resources;
            this.documents = documents;
            this.sendOrder = sendOrder;
            this.shoppingCart = shoppingCart;
            this.orderDataProvider = orderDataProvider;
            this.resultUrlFactory = resultUrlFactory;
        }

        public async Task<SubmitOrderResult> PayBySavedCard3dsi(SubmitOrderRequest orderRequest)
        {
            var savedCardId = orderRequest?.PaymentMethod?.Card;
            if (string.IsNullOrEmpty(savedCardId))
            {
                logger.LogError("PayOrderBySavedCard", "No saved card Id was given");
                return ReturnResult(false, error: "Kadena.OrderByCardFailed.ApprovalFailed");
            }

            logger.LogInfo("PayOrderBySavedCard", "Info", "Attempting to pay by saved card with Id " + savedCardId);

            var orderData = await orderDataProvider.GetSubmitOrderData(orderRequest);
            orderData.PaymentOption.TokenId = savedCardId;
            orderData.PaymentOption.PaymentGatewayCustomerCode = resources.GetSettingsKey("KDA_CreditCard_Code");

            var sendOrderResult = await sendOrder.SubmitOrderData(orderData);

            if (!(sendOrderResult?.Success ?? false))
            {
                return ReturnResult(success: false, 
                                    orderId: sendOrderResult?.Payload, 
                                    error: "Kadena.OrderByCardFailed.SaveDataToMicroserviceFailed");
            }

            shoppingCart.RemoveCurrentItemsFromStock();
            shoppingCart.ClearCart();

            return ReturnResult(success: true, 
                                orderId: sendOrderResult.Payload);
        }

        private SubmitOrderResult ReturnResult(bool success, string orderId = "", string error = "")
        {
            return new SubmitOrderResult
            {
                Success = success,
                RedirectURL = resultUrlFactory.GetCardPaymentResultPageUrl(success, orderId, error: error)
            };
        }
    }
}
