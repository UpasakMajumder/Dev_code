using System;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.CreditCard;
using Kadena2.MicroserviceClients.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ISubmissionIdProvider submissionProvider;
        private readonly IUserDataServiceClient userClient;
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoSiteProvider kenticoSite;
        

        public SubmissionService(ISubmissionIdProvider submissionProvider, IUserDataServiceClient userClient, IKenticoUserProvider kenticoUsers, IKenticoSiteProvider kenticoSite)
        {
            if (submissionProvider == null)
            {
                throw new ArgumentNullException(nameof(submissionProvider));
            }
            if (userClient == null)
            {
                throw new ArgumentNullException(nameof(userClient));
            }
            if (kenticoUsers == null)
            {
                throw new ArgumentNullException(nameof(kenticoUsers));
            }
            if (kenticoSite == null)
            {
                throw new ArgumentNullException(nameof(kenticoSite));
            }

            this.submissionProvider = submissionProvider;
            this.userClient = userClient;
            this.kenticoUsers = kenticoUsers;
            this.kenticoSite = kenticoSite;
        }

        public Submission GenerateSubmissionId()
        {
            int siteId = kenticoSite.GetKenticoSite().Id;
            int userId = kenticoUsers.GetCurrentUser().UserId;
            int customerId = kenticoUsers.GetCurrentCustomer().Id;

            var oldSubmissions = submissionProvider.GetSubmissions(siteId, userId, customerId);

            foreach (var oldSubmission in oldSubmissions)
            {
                submissionProvider.DeleteSubmission(oldSubmission.SubmissionId);
            }

            var submission = new Submission()
            {
                SubmissionId = Guid.NewGuid(),
                AlreadyUsed = false,
                UserId = userId,
                CustomerId = customerId,
                SiteId = siteId
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
    }
}