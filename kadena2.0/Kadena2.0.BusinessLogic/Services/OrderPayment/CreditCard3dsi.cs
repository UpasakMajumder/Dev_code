using Kadena.Dto.CreditCard.MicroserviceRequests;
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
using Kadena.BusinessLogic.Factories;

namespace Kadena2.BusinessLogic.Services.OrderPayment
{
    public class CreditCard3dsi : ICreditCard3dsi
    {
        private readonly ISubmissionService submissionService;
        private readonly IUserDataServiceClient userClient;
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoLogger logger;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;
        private readonly IKenticoSiteProvider kenticoSite;
        private readonly ISendSubmitOrder sendOrder;
        private readonly IShoppingCartProvider shoppingCart;
        private readonly IGetOrderDataService orderDataProvider;
        private readonly IOrderResultPageUrlFactory resultUrlFactory;

        public CreditCard3dsi(ISubmissionService submissionService, IUserDataServiceClient userClient, IKenticoUserProvider kenticoUsers, IKenticoLogger logger, IKenticoResourceService resources, IKenticoDocumentProvider documents, IKenticoSiteProvider kenticoSite, ISendSubmitOrder sendOrder, IShoppingCartProvider shoppingCart, IGetOrderDataService orderDataProvider, IOrderResultPageUrlFactory resultUrlFactory)
        {
            if (submissionService == null)
            {
                throw new ArgumentNullException(nameof(submissionService));
            }
            if (userClient == null)
            {
                throw new ArgumentNullException(nameof(userClient));
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
            if (resultUrlFactory == null)
            {
                throw new ArgumentNullException(nameof(resultUrlFactory));
            }

            this.submissionService = submissionService;
            this.userClient = userClient;
            this.kenticoUsers = kenticoUsers;
            this.logger = logger;
            this.resources = resources;
            this.documents = documents;
            this.kenticoSite = kenticoSite;
            this.sendOrder = sendOrder;
            this.shoppingCart = shoppingCart;
            this.orderDataProvider = orderDataProvider;
            this.resultUrlFactory = resultUrlFactory;
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

        /// <summary>
        /// Service for handling SaveToken request from 3DSi
        /// </summary>
        public async Task<bool> SaveToken(SaveTokenData tokenData)
        {
            if (tokenData == null || tokenData.Approved == 0)
            {
                logger.LogError("3DSi SaveToken", "Empty or not approved token data received from 3DSi");
                return false;
            }
            
            var submission = submissionService.GetSubmission(tokenData.SubmissionID);

            if (submission == null || !submission.AlreadyVerified)
            {
                logger.LogError("3DSi SaveToken", $"Unknown or already used submissionId : {tokenData.SubmissionID}");
                return false;
            }

            logger.LogInfo("3DSi SaveToken", "Info", $"Received SaveTokenData request, status: " + tokenData?.SubmissionStatusMessage);

            var tokenId = await SaveTokenToUserData(submission.UserId.ToString(), tokenData);

            if (string.IsNullOrEmpty(tokenId))
            {
                MarkSubmissionProcessed(submission, false, string.Empty);
                return false;
            }

            var orderData = JsonConvert.DeserializeObject<OrderDTO>(submission.OrderJson, SerializerConfig.CamelCaseSerializer);
            orderData.PaymentOption.TokenId = tokenId;
            orderData.PaymentOption.PaymentGatewayCustomerCode = resources.GetSettingsKey("KDA_CreditCard_Code");

            var sendOrderResult = await sendOrder.SubmitOrderData(orderData);

            if (!(sendOrderResult?.Success ?? false))
            {
                MarkSubmissionProcessed(submission, false, sendOrderResult?.Payload);
                return false;
            }

            var orderSuccess = ClearKenticoShoppingCart(submission.UserId, submission.SiteId);

            MarkSubmissionProcessed(submission, orderSuccess, sendOrderResult?.Payload);

            return orderSuccess;
        }

        private void MarkSubmissionProcessed(Submission submission, bool orderSuccess, string orderId)
        {
            var redirectUrl = resultUrlFactory.GetCardPaymentResultPageUrl(orderSuccess, orderId);
            submissionService.SetAsProcessed(submission, redirectUrl);
        }

        private bool ClearKenticoShoppingCart(int userId, int siteId)
        {
            var shoppingCartId = shoppingCart.GetShoppingCartId(userId, siteId);

            if (shoppingCartId == 0)
            {
                logger.LogInfo("PayOrderByCard", "Error", $"Failed to clean shopping cart in Kentico");
                return false;
            }
            
            shoppingCart.RemoveCurrentItemsFromStock(shoppingCartId);
            shoppingCart.ClearCart(shoppingCartId);

            return true;
        }

        /// <summary>
        /// Saves the Token into UserData microservice
        /// </summary>
        public async Task<string> SaveTokenToUserData(string userId, SaveTokenData token)
        {
            var saveTokenRequest = new SaveCardTokenRequestDto
            {
                // TODO some more properties needed for storing card:
                // flag if treat card as stored
                // last 4 digits
                // username 
                UserId = userId,
                Token = token.Token
            };

            var result = await userClient.SaveCardToken(saveTokenRequest);

            if (result == null || !result.Success || string.IsNullOrWhiteSpace(result.Payload))
            {
                var error = "Failed to call UserData microservice to SaveToken. " + result?.ErrorMessages;
                logger.LogError("SaveToken", error);
                return null;
            }

            logger.LogInfo("3DSi SaveToken", "info", "Token saved to User data microservice");
            return result.Payload;
        }

        /// <summary>
        /// Handles FE periodical request if payment processeed
        /// </summary>
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

        public async Task SaveCreditCard(SaveCardData cardData)
        {
            var saveRequest = new SaveCardTokenRequestDto
            {
                CardNumber = cardData.CardNumber,
                Name = cardData.Name, // TODO some user friendly name ?
                //Token = cardData.Token,
                TokenExpirationDate = "TODO, will FE pass it ?",
                UserId = kenticoUsers.GetCurrentUser().UserId.ToString(),
            };

            var tokenId = await userClient.SaveCardToken(saveRequest);
        }
    }
}
