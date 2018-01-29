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
            var redirectUrl = documents.GetDocumentUrl(insertCardUrl);
            var uri = new Uri(redirectUrl).AddParameter("submissionId", newSubmission.SubmissionId.ToString());

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

            

            string userId = submission.UserId.ToString();

            var tokenId = await SaveTokenToUserData(userId, tokenData);

            if (string.IsNullOrEmpty(tokenId))
            {
                logger.LogError("3DSi SaveToken", "Saving token to microservice failed");
                return false;
            }

            var orderData = JsonConvert.DeserializeObject<OrderDTO>(submission.OrderJson, SerializerConfig.CamelCaseSerializer);


            var authorizeResponse = await AuthorizeAmount(userId, tokenId, tokenData.Token, orderData);
            if (!(authorizeResponse?.Succeeded ?? false))
            {
                logger.LogError("AuthorizeAmount", $"AuthorizeAmount failed, response:{Environment.NewLine}{authorizeResponse}");
                return false;
            }


            logger.LogInfo("AuthorizeAmount", "Info", $"AuthorizeAmount OK, response:{Environment.NewLine}{authorizeResponse}");

            // TODO pass data needet to finish the order, are there some ?
            //authorizeResponse.TransactionKey ?

            var sendToOrderServiceResult = await SendOrderToMicroservice(orderData);

            if (!(sendToOrderServiceResult?.Success ?? false))
            {
                return false;
            }

            submissionService.SetAsProcessed(submission);
            return true;
        }

        private async Task<SubmitOrderResult> SendOrderToMicroservice(OrderDTO orderData)
        {
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

        private async Task<AuthorizeAmountResponseDto> AuthorizeAmount(string userId, string tokenId, string token, OrderDTO orderData)
        {
            var url = resources.GetSettingsKey("KDA_PaymentMicroserviceEndpoint");

            var authorizeAmountRequest = new AuthorizeAmountRequestDto
            {
                User = new UserDto
                {
                    UserInternalId = userId,
                    UserPaymentSystemCode = orderData.PaymentOption.KenticoPaymentOptionID.ToString()
                },

                PaymentData = new PaymentDataDto
                {
                    CardTokenId = tokenId,
                    Token = token,
                    PaymentProvider = string.Empty,
                    TransactionKey = string.Empty // TODO
                },

                AdditionalAmounts = new AdditionalAmountsDto
                {
                    ShippingAmount = orderData?.TotalShipping ?? 0.0m,
                    SalesTaxAmount = 0.0m, // TODO
                    ShippingTax = 0.0m // TODO
                },
                SapDetails = new SapDetailsDto
                {
                    InvoiceNumber = orderData.PaymentOption.PONumber,
                    // TODO some other properties ?
                },

                Currency = orderData.OrderCurrency.KenticoCurrencyID,
                TotalAmount = orderData.TotalPrice
            };

            var result = await paymentClient.AuthorizeAmount(authorizeAmountRequest);

            if (!(result?.Success ?? false) || result.Payload == null)
            {
                var error = "Failed to call Payment microservice to AuthorizeAmount. " + result?.ErrorMessages;
                logger.LogError("AuthorizeAmount", error);
                return null;
            }

            return result.Payload;
        }

        public bool CreditcardSaved(string submissionId)
        {
            var submission = submissionService.GetSubmission(submissionId);

            int siteId = kenticoSite.GetKenticoSite().Id;
            int userId = kenticoUsers.GetCurrentUser().UserId;
            int customerId = kenticoUsers.GetCurrentCustomer().Id;

            var submissonOwnerChecked = submission.CheckOwner(siteId, userId, customerId);

            if (submission == null || !submission.AlreadyVerified)
            {
                return false;
            }

            if (submission.Processed)
            {
                submissionService.DeleteProcessedSubmission(submission);
                return true;
            }

            return false;
        }
    }
}
