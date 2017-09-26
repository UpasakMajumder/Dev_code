using System;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.CreditCard;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.Dto.CreditCard.MicroserviceRequests;
using System.Threading.Tasks;
using Kadena.Dto.CreditCard;

namespace Kadena.WebAPI.Services
{
    public class CreditCardService : ICreditCardService
    {
        private readonly ISubmissionIdProvider submissionProvider;
        private readonly IUserDataServiceClient userClient;
        private readonly IPaymentServiceClient paymentClient;
        private readonly IKenticoResourceService resources;

        public CreditCardService(ISubmissionIdProvider submissionProvider, IUserDataServiceClient userClient, IPaymentServiceClient paymentClient, IKenticoResourceService resources)
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

            this.submissionProvider = submissionProvider;
            this.userClient = userClient;
            this.paymentClient = paymentClient;
            this.resources = resources;
        }

        public Guid GenerateSubmissionId()
        {
            var submission = new Submission()
            {
                SubmissionId = Guid.NewGuid(),
                AlreadyUsed = false
            };

            submissionProvider.SaveSubmission(submission);
            return submission.SubmissionId;
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

            var tokenId =  await CallSaveToken(userId, tokenData.Token); // TODO get user id from custom table if necessary

            await CallAuthorizeAmount(userId, tokenId, tokenData.Token);

            // TODO pass data needet to finish the order

            return true;
        }


        private async Task<string> CallSaveToken(string userId, string token)
        {
            var url = resources.GetSettingsKey("URL_TO_SAVETOKEN");

            var saveTokenRequest = new SaveCardTokenRequestDto
            {
                // TODO lot of properties here

                UserId = userId,
                Token = token
            };

            var result = await userClient.SaveCardToken(url, saveTokenRequest);

            var tokenId = result.Payload.Result;

            // TODO handle calling error

            // TODO log result

            return tokenId;
        }



        private async Task CallAuthorizeAmount(string userId, string tokenId, string token)
        {
            var url = resources.GetSettingsKey("URL_TO_AUTHORIZE_AMOUNT");

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

            var result = await paymentClient.AuthorizeAmount(url, authorizeAmountRequest);

            // TODO handle calling error

            // TODO log result
        }
    }
}