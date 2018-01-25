using Newtonsoft;
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

namespace Kadena2.BusinessLogic.Services.OrderPayment
{
    public class CreditCard3dsi : ICreditCard3dsi
    {
        private readonly ISubmissionIdProvider submissionProvider;
        private readonly IUserDataServiceClient userClient;
        private readonly IPaymentServiceClient paymentClient;
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoLogger logger;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;
        private readonly IGetOrderDataService orderDataProvider;


        public CreditCard3dsi(ISubmissionIdProvider submissionProvider, IUserDataServiceClient userClient, IPaymentServiceClient paymentClient, IKenticoUserProvider kenticoUsers, IKenticoLogger logger, IKenticoResourceService resources, IKenticoDocumentProvider documents, IGetOrderDataService orderData)
        {
            if (submissionProvider == null)
            {
                throw new ArgumentNullException(nameof(submissionProvider));
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
            if (orderData == null)
            {
                throw new ArgumentNullException(nameof(orderData));
            }

            this.submissionProvider = submissionProvider;
            this.userClient = userClient;
            this.paymentClient = paymentClient;
            this.kenticoUsers = kenticoUsers;
            this.logger = logger;
            this.resources = resources;
            this.documents = documents;
            this.orderDataProvider = orderData;
        }

        public async Task<SubmitOrderResult> PayByCard3dsi(OrderDTO orderData)
        {
            var insertCardUrl = resources.GetSettingsKey("KDA_CreditCard_InsertCardDetailsURL");

            var newSubmission = GenerateSubmissionId();
            newSubmission.OrderJson = "{}";//Newtonsoft.Json.Ser

            return await Task.FromResult(new SubmitOrderResult
                {
                    Success = true,
                    RedirectURL = documents.GetDocumentUrl(insertCardUrl)
                }
            );
        }

        public Submission GenerateSubmissionId()
        {
            return new Submission()
            {
                SubmissionId = Guid.NewGuid(),
                AlreadyUsed = false,
                UserId = kenticoUsers.GetCurrentUser().UserId,
                CustomerId = kenticoUsers.GetCurrentCustomer().Id,
                // TODO site ID
                Processed = false
            };
        }

        public bool VerifySubmissionId(string submissionId)
        {
            var submissionGuid = Guid.Empty;
            if (!Guid.TryParse(submissionId, out submissionGuid))
            {
                return false;
            }

            var submission = submissionProvider.GetSubmission(submissionGuid);

            if (submission != null && !submission.AlreadyUsed)
            {
                submission.AlreadyUsed = true;
                submissionProvider.SaveSubmission(submission);
                return true;
            }

            return false;
        }

        public async Task<bool> SaveToken(SaveTokenData tokenData)
        {
            if (tokenData == null || !tokenData.Approved)
            {
                return false;
            }

            var submissionGuid = Guid.Empty;
            if (!Guid.TryParse(tokenData.SubmissionID, out submissionGuid))
            {
                return false;
            }

            var submission = submissionProvider.GetSubmission(submissionGuid);

            if (submission == null || !submission.AlreadyUsed)
            {
                return false;
            }

            string userId = submission.UserId.ToString();

            var tokenId = await SaveTokenToUserData(userId, tokenData.Token);

            if (string.IsNullOrEmpty(tokenId))
            {
                logger.LogError("3DSi SaveToken", "Saving token to microservice failed");
                return false;
            }

            var authorizeResponse = await AuthorizeAmount(userId, tokenId, tokenData.Token);
            if (!(authorizeResponse?.Succeeded ?? false))
            {
                logger.LogError("AuthorizeAmount", $"AuthorizeAmount failed, response:{Environment.NewLine}{authorizeResponse}");
                return false;
            }


            logger.LogInfo("AuthorizeAmount", "Info", $"AuthorizeAmount OK, response:{Environment.NewLine}{authorizeResponse}");

            // TODO pass data needet to finish the order, are there some ?
            //authorizeResponse.TransactionKey

            submission.Processed = true;
            submissionProvider.SaveSubmission(submission);

            SubmitOrderRequest submitOrderRequest = null; // TODO deserialize from submission
            var orderData = await orderDataProvider.GetSubmitOrderData(submitOrderRequest);

            return true;
        }

        /// <summary>
        /// Saves the Token into UserData microservice
        /// </summary>
        private async Task<string> SaveTokenToUserData(string userId, string token)
        {
            var url = resources.GetSettingsKey("KDA_UserdataMicroserviceEndpoint");

            var saveTokenRequest = new SaveCardTokenRequestDto
            {
                // TODO lot of properties here

                UserId = userId,
                Token = token
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

        private async Task<AuthorizeAmountResponseDto> AuthorizeAmount(string userId, string tokenId, string token)
        {
            var url = resources.GetSettingsKey("KDA_PaymentMicroserviceEndpoint");

            var authorizeAmountRequest = new AuthorizeAmountRequestDto
            {
                // TODO lot of properties here
                PaymentData = new PaymentDataDto()
                {
                    CardTokenId = tokenId,
                    Token = token,
                    PaymentProvider = string.Empty
                    //TransactionKey = 
                    // TODO
                },

                User = new UserDto()
                {
                    UserInternalId = userId,
                    UserPaymentSystemCode = string.Empty
                }
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
            var submissionGuid = Guid.Empty;
            if (!Guid.TryParse(submissionId, out submissionGuid))
            {
                return false;
            }

            var submission = submissionProvider.GetSubmission(submissionGuid);

            if (submission == null || !submission.AlreadyUsed)
            {
                return false;
            }

            return submission.Processed;
        }
    }
}
