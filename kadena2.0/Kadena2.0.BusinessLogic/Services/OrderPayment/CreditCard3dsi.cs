using Kadena.Dto.CreditCard.MicroserviceRequests;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models.CreditCard;
using Kadena.Models.SubmitOrder;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.BusinessLogic.Contracts.Orders;
using Kadena2.BusinessLogic.Contracts.OrderPayment;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Threading.Tasks;
using Kadena.Helpers;
using Newtonsoft.Json;
using Kadena.BusinessLogic.Contracts;
using Kadena2.Helpers;
using Kadena.BusinessLogic.Factories;
using System.Globalization;
using Kadena.Models.SiteSettings;

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
            var orderData = await orderDataProvider.GetSubmitOrderData(orderRequest);
            var orderJson = JsonConvert.SerializeObject(orderData, SerializerConfig.CamelCaseSerializer);
            var newSubmission = submissionService.GenerateNewSubmission(orderJson);
            var url = GetInsertCardDetailsUrl(newSubmission.SubmissionId.ToString());

            return new SubmitOrderResult
            {
                Success = true,
                RedirectURL = url
            };
        }


        private string GetInsertCardDetailsUrl(string submissionId)
        {
            var insertCardUrl = resources.GetSiteSettingsKey(Settings.KDA_CreditCard_InsertCardDetailsURL);
            var redirectUrl = documents.GetDocumentUrl(insertCardUrl, absoluteUrl: true);
            var uri = new Uri(redirectUrl, UriKind.Absolute).AddParameter("submissionId", submissionId);
            return uri.ToString();
        }

        /// <summary>
        /// Service for handling SaveToken request from 3DSi
        /// </summary>
        public async Task<bool> SaveToken(SaveTokenData tokenData)
        {
            if (tokenData == null)
            {
                logger.LogError("3DSi SaveToken", "Empty or unknown format SaveTokenData");
                return false;
            }

            var submission = submissionService.GetSubmission(tokenData.SubmissionID);

            if (submission == null || !submission.AlreadyVerified)
            {
                logger.LogError("3DSi SaveToken", $"Unknown or already used submissionId : {tokenData.SubmissionID}");
                return false;
            }

            if (tokenData.Approved == 0 || string.IsNullOrEmpty(tokenData.Token))
            {
                logger.LogError("3DSi SaveToken", "Empty or not approved token data received from 3DSi. " + tokenData?.SubmissionStatusMessage);
                MarkSubmissionProcessed(submission, 
                                        orderSuccess: false, 
                                        orderId: string.Empty, 
                                        error: "Kadena.OrderByCardFailed.ApprovalFailed");
                return false;
            }

            logger.LogInfo("3DSi SaveToken", "Info", $"Received SaveTokenData request, status: " + tokenData?.SubmissionStatusMessage);

            var tokenId = await SaveTokenToUserData(submission, tokenData);

            if (string.IsNullOrEmpty(tokenId))
            {
                MarkSubmissionProcessed(submission, 
                                        orderSuccess: false, 
                                        orderId: string.Empty, 
                                        error: "Kadena.OrderByCardFailed.PlaceOrderFailed");
                return false;
            }

            var orderData = JsonConvert.DeserializeObject<OrderDTO>(submission.OrderJson, SerializerConfig.CamelCaseSerializer);
            orderData.PaymentOption.TokenId = tokenId;
            orderData.PaymentOption.PaymentGatewayCustomerCode = resources.GetSiteSettingsKey(Settings.KDA_CreditCard_Code);

            var sendOrderResult = await sendOrder.SubmitOrderData(orderData);

            if (sendOrderResult?.Success != true)
            {
                MarkSubmissionProcessed(submission, 
                                        orderSuccess: false, 
                                        orderId: sendOrderResult?.OrderId, 
                                        error: "Kadena.OrderByCardFailed.PlaceOrderFailed");
                return false;
            }

            var orderSuccess = ClearKenticoShoppingCart(submission.UserId, submission.SiteId);
            var error = orderSuccess ? string.Empty : "Kadena.OrderByCardFailed.PlaceOrderFailed";

            MarkSubmissionProcessed(submission, orderSuccess, 
                                    orderId: sendOrderResult?.OrderId, 
                                    error : error);

            return orderSuccess;
        }

        private void MarkSubmissionProcessed(Submission submission, bool orderSuccess, string orderId, string error = "")
        {
            var redirectUrl = resultUrlFactory.GetCardPaymentResultPageUrl(orderSuccess, orderId, submission.SubmissionId.ToString(), error);
            submissionService.SetAsProcessed(submission, orderSuccess, redirectUrl, error);
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
        public async Task<string> SaveTokenToUserData(Submission submission, SaveTokenData token)
        {
            SaveCardTokenRequestDto saveTokenRequest;

            if (string.IsNullOrEmpty(submission.SaveCardJson))
            {
                saveTokenRequest = new SaveCardTokenRequestDto
                {
                    UserId = submission.UserId.ToString(),
                    SaveToken = false
                };
            }
            else
            {
                saveTokenRequest = JsonConvert.DeserializeObject<SaveCardTokenRequestDto>(submission.SaveCardJson);
                saveTokenRequest.Name = $"{token.CardType} *-{token.CardEnd}";
            }

            
            saveTokenRequest.Token = token.Token;
            saveTokenRequest.CardExpirationDate = ParseCardExpiration(token.ExpirationDate);

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

        private DateTime ParseCardExpiration(string expiration)
        {
            // 3DSi support confirmed that format will always be yyMM
            var cultureInfo = new CultureInfo(CultureInfo.InvariantCulture.LCID);
            cultureInfo.Calendar.TwoDigitYearMax = 2099;
            return DateTime.ParseExact(expiration, new string[] { "yyMM" }, cultureInfo, DateTimeStyles.None);
        }

        /// <summary>
        /// Handles FE periodical request if payment processeed
        /// </summary>
        public string CreditcardSaved(string submissionId)
        {
            var submission = submissionService.GetSubmission(submissionId);

            if (submission != null && submission.AlreadyVerified && submission.Processed && submissionService.CheckOwner(submission))
            {
                if (submission.Success)
                {
                    submissionService.DeleteProcessedSubmission(submission);
                }

                return submission.RedirectUrl;
            }

            return string.Empty;
        }

        public void MarkCardAsSaved(SaveCardData cardData)
        {
            if (cardData?.Save != true)
            {
                return;
            }

            var saveRequest = new SaveCardTokenRequestDto
            {
                CardNumber = cardData.CardNumber,
                Name = cardData.Name, 
                UserId = kenticoUsers.GetCurrentUser().UserId.ToString(),
                SaveToken = true
            };

            var saveJson = JsonConvert.SerializeObject(saveRequest, SerializerConfig.CamelCaseSerializer);
            submissionService.SetSaveCardJson(cardData.SubmissionID, saveJson);
        }

        public string RetryInsertCardDetails(string submissionId)
        {
            var newSubmissionId = submissionService.RenewSubmission(submissionId);
            return GetInsertCardDetailsUrl(newSubmissionId.ToString());
        }
    }
}
