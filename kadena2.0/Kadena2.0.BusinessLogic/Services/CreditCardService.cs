using System;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.CreditCard;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.Dto.CreditCard.MicroserviceRequests;
using System.Threading.Tasks;
using Kadena.Dto.CreditCard;
using Kadena.Dto.CreditCard.MicroserviceResponses;

namespace Kadena.BusinessLogic.Services
{
    public class CreditCardService : ICreditCardService
    {
        private readonly ISubmissionIdProvider submissionProvider;
        private readonly IUserDataServiceClient userClient;
        private readonly IPaymentServiceClient paymentClient;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoLogger logger;

        public CreditCardService(ISubmissionIdProvider submissionProvider, IUserDataServiceClient userClient, IPaymentServiceClient paymentClient, IKenticoResourceService resources, IKenticoUserProvider kenticoUsers, IKenticoLogger logger)
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
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (kenticoUsers == null)
            {
                throw new ArgumentNullException(nameof(kenticoUsers));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.submissionProvider = submissionProvider;
            this.userClient = userClient;
            this.paymentClient = paymentClient;
            this.resources = resources;
            this.kenticoUsers = kenticoUsers;
            this.logger = logger;
        }

        public Submission GenerateSubmissionId()
        {
            var submission = new Submission()
            {
                SubmissionId = Guid.NewGuid(),
                AlreadyUsed = false,
                UserId = kenticoUsers.GetCurrentUser().UserId
            };

            submissionProvider.SaveSubmission(submission);
            return submission;
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

            submission.TokenSavedAndAuthorized = true;
            submissionProvider.SaveSubmission(submission);

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
                    Token = token
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

            return submission.TokenSavedAndAuthorized;
        }
    }
}