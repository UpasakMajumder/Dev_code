using Kadena.Dto.CreditCard;
using Kadena.Dto.CreditCard.MicroserviceRequests;
using Kadena.Dto.CreditCard.MicroserviceResponses;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models.CreditCard;
using Kadena.Models.SubmitOrder;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2._0.BusinessLogic.Contracts.Orders;
using Kadena2.BusinessLogic.Contracts.OrderPayment;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Threading.Tasks;
using Kadena.Helpers;
using Newtonsoft.Json;
using Kadena.BusinessLogic.Contracts;
using Kadena2.Helpers;

namespace Kadena2.BusinessLogic.Services.OrderPayment
{
    public class CreditCard3dsi : ICreditCard3dsi
    {
        private readonly ISubmissionService submissionService;
        private readonly IUserDataServiceClient userClient;
        private readonly IPaymentServiceClient paymentClient;
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoLogger logger;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;
        private readonly IKenticoSiteProvider kenticoSite;
        private readonly ISendSubmitOrder sendOrder;
        private readonly IShoppingCartProvider shoppingCart;
        private readonly IGetOrderDataService orderDataProvider;

        public CreditCard3dsi(ISubmissionService submissionService, IUserDataServiceClient userClient, IPaymentServiceClient paymentClient, IKenticoUserProvider kenticoUsers, IKenticoLogger logger, IKenticoResourceService resources, IKenticoDocumentProvider documents, IKenticoSiteProvider kenticoSite, ISendSubmitOrder sendOrder, IShoppingCartProvider shoppingCart, IGetOrderDataService orderDataProvider)
        {
            if (submissionService == null)
            {
                throw new ArgumentNullException(nameof(submissionService));
            }
            if (userClient == null)
            {
                throw new ArgumentNullException(nameof(userClient));
            }
            if (paymentClient == null)
            {
                throw new ArgumentNullException(nameof(paymentClient));
            }
            if (kenticoUsers == null)
            {
                throw new ArgumentNullException(nameof(kenticoUsers));
            }
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
            if (kenticoSite == null)
            {
                throw new ArgumentNullException(nameof(kenticoSite));
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

            this.submissionService = submissionService;
            this.userClient = userClient;
            this.paymentClient = paymentClient;
            this.kenticoUsers = kenticoUsers;
            this.logger = logger;
            this.resources = resources;
            this.documents = documents;
            this.kenticoSite = kenticoSite;
            this.sendOrder = sendOrder;
            this.shoppingCart = shoppingCart;
            this.orderDataProvider = orderDataProvider;
        }

        public async Task<SubmitOrderResult> PayByCard3dsi(SubmitOrderRequest orderRequest)
        {
            var insertCardUrl = resources.GetSettingsKey("KDA_CreditCard_InsertCardDetailsURL");
            var orderData = await orderDataProvider.GetSubmitOrderData(orderRequest);
            var orderJson = JsonConvert.SerializeObject(orderData, SerializerConfig.CamelCaseSerializer);
            var newSubmission = submissionService.GenerateNewSubmission(orderJson);
            var redirectUrl = documents.GetDocumentUrl(insertCardUrl, absoluteUrl: true);
            var uri = new Uri(redirectUrl, UriKind.Absolute).AddParameter("submissionId", newSubmission.SubmissionId.ToString());

            return new SubmitOrderResult
            {
                Success = true,
                RedirectURL = uri.ToString()
            };
        }

        public async Task<bool> SaveToken(SaveTokenData tokenData)
        {
            if (tokenData == null || !tokenData.Approved)
            {
                return false;
            }
            
            var submission = submissionService.GetSubmission(tokenData.SubmissionID);

            if (submission == null || !submission.AlreadyVerified)
            {
                return false;
            }

            var tokenId = await SaveTokenToUserData(submission.UserId.ToString(), tokenData);

            if (string.IsNullOrEmpty(tokenId))
            {
                logger.LogError("3DSi SaveToken", "Saving token to microservice failed");
                return false;
            }

            var orderData = JsonConvert.DeserializeObject<OrderDTO>(submission.OrderJson, SerializerConfig.CamelCaseSerializer);
            orderData.PaymentOption.TokenId = tokenId;
            orderData.PaymentOption.PaymentGatewayCustomerCode = resources.GetSettingsKey("KDA_CreditCard_Code");

            var sendOrderResult = await sendOrder.SubmitOrderData(orderData);

            if (!(sendOrderResult?.Success ?? false))
            {
                logger.LogError("PayOrderByCard", "Failed to save order to microservice.  " + sendOrderResult?.Error?.Message);
                submissionService.SetAsProcessed(submission, GetOrderResultPageUrl(sendOrderResult));
                return false;
            }

            logger.LogInfo("PayOrderByCard", "info", $"Order #{sendOrderResult.Payload} was saved into microservice");

            shoppingCart.RemoveCurrentItemsFromStock();
            shoppingCart.ClearCart();
            submissionService.SetAsProcessed(submission, GetOrderResultPageUrl(sendOrderResult));

            return true;
        }

        private string GetOrderResultPageUrl(SubmitOrderResult submitOrderResult)
        {
            var redirectUrlBase = resources.GetSettingsKey("KDA_CreditCard_PaymentResultPage");
            var redirectUrlBaseLocalized = documents.GetDocumentUrl(redirectUrlBase);
            var redirectUrl = $"{redirectUrlBaseLocalized}?success={submitOrderResult?.Success ?? false}".ToLower();
            if (submitOrderResult?.Success ?? false)
            {
                redirectUrl += "&order_id=" + submitOrderResult.Payload;
            }
            return redirectUrl;
        }


        /// <summary>
        /// Saves the Token into UserData microservice
        /// </summary>
        private async Task<string> SaveTokenToUserData(string userId, SaveTokenData token)
        {
            var url = resources.GetSettingsKey("KDA_UserdataMicroserviceEndpoint");

            var saveTokenRequest = new SaveCardTokenRequestDto
            {
                // TODO some more properties ?
                UserId = userId,
                Token = token.Token
            };

            var result = await userClient.SaveCardToken(saveTokenRequest);

            if (result == null || !result.Success || result.Payload == null)
            {
                var error = "Failed to call UserData microservice to SaveToken. " + result?.ErrorMessages;
                logger.LogError("SaveToken", error);
                return null;
            }

            return result.Payload.Result;
        }

        public string CreditcardSaved(string submissionId)
        {
            var submission = submissionService.GetSubmission(submissionId);

            if (submission?.AlreadyVerified ?? false)
            {
                int siteId = kenticoSite.GetKenticoSite().Id;
                int userId = kenticoUsers.GetCurrentUser().UserId;
                int customerId = kenticoUsers.GetCurrentCustomer().Id;
                var submissonOwnerChecked = submission.CheckOwner(siteId, userId, customerId);

                if (submissonOwnerChecked && submission.Processed)
                {
                    var redirectUrl = submission.RedirectUrl;
                    submissionService.DeleteProcessedSubmission(submission);
                    return redirectUrl;
                }
            }

            return string.Empty;
        }
    }
}
