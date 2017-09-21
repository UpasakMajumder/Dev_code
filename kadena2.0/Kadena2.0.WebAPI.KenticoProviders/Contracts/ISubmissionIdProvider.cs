using System;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface ISubmissionIdProvider
    {
        void StoreSubmissionId(Guid submissionId);
        bool VerifySubmissionId(Guid submissionId);
        void DeleteSubmissionId(Guid submissionId);
    }
}
