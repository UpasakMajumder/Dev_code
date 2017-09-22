using System;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.CreditCard;

namespace Kadena.WebAPI.Services
{
    public class CreditCardService : ICreditCardService
    {
        private readonly ISubmissionIdProvider submissionProvider;

        public CreditCardService(ISubmissionIdProvider submissionProvider)
        {
            if (submissionProvider == null)
            {
                throw new ArgumentNullException(nameof(submissionProvider));
            }

            this.submissionProvider = submissionProvider;
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

        public bool VerifySubmissionId(Guid submissionId)
        {
            var submission = submissionProvider.GetSubmission(submissionId);

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