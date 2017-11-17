using Kadena.Models.CreditCard;
using System;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface ISubmissionIdProvider
    {
        void SaveSubmission(Submission submission);
        Submission GetSubmission(Guid submissionId);
        void DeleteSubmission(Guid submissionId);
    }
}
