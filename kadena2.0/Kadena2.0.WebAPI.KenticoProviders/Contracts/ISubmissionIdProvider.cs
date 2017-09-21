using System;

namespace Kadena2.WebAPI.KenticoProviders.Contracts
{
    public interface ISubmissionIdProvider
    {
        void StoreSubmissionId(Guid submissionId);
        bool VerifySubmissionId(Guid submissionId);
        void DeleteSubmissionId(Guid submissionId);
    }
}
