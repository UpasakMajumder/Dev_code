using System;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;

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
            var submissionid = Guid.NewGuid();
            submissionProvider.StoreSubmissionId(submissionid);
            return submissionid;
        }

        public bool VerifySubmissionId(Guid submissionId)
        {
            var exists = submissionProvider.VerifySubmissionId(submissionId);

            if (exists)
            {
                submissionProvider.DeleteSubmissionId(submissionId);
            }

            return exists;
        }
    }
}