using System;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.CreditCard;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.Dto.CreditCard.MicroserviceRequests;
using System.Threading.Tasks;
using Kadena.Dto.CreditCard;

namespace Kadena.BusinessLogic.Services
{
    public class CreditCardService : ICreditCardService
    {
        private readonly ISubmissionIdProvider submissionProvider;
        private readonly IUserDataServiceClient userClient;
        private readonly IPaymentServiceClient paymentClient;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoLogger logger;

        public CreditCardService(ISubmissionIdProvider submissionProvider, IUserDataServiceClient userClient, IPaymentServiceClient paymentClient, IKenticoResourceService resources, IKenticoLogger logger)
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
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.submissionProvider = submissionProvider;
            this.userClient = userClient;
            this.paymentClient = paymentClient;
            this.resources = resources;
            this.logger = logger;
        }

        public Submission GenerateSubmissionId()
        {
            var submission = new Submission()
            {
                SubmissionId = Guid.NewGuid(),
                AlreadyUsed = false
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

            submissionProvider.DeleteSubmission(submissionGuid);

            string userId = submission.UserId.ToString();

            var tokenId = await CallSaveToken(userId, tokenData.Token); // TODO get user id from custom table if necessary

            if (string.IsNullOrEmpty(tokenId))
            {
                // TODO log and quit with error
            }

            await CallAuthorizeAmount(userId, tokenId, tokenData.Token);

            // TODO check if Auth failed

            // TODO pass data needet to finish the order

            // TODO notify FE waiting request that it is done or failed

            return true;
        }


        private async Task<string> CallSaveToken(string userId, string token)
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



        private async Task CallAuthorizeAmount(string userId, string tokenId, string token)
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

            if (result == null || !result.Success || result.Payload == null)
            {
                var error = "Failed to call Payment microservice to AuthorizeAmount. " + result?.ErrorMessages;
                logger.LogError("SaveToken", error);
                return;
            }

            //return result.Payload.
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

            return false; // TODO
        }
    }
}