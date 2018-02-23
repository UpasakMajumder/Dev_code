using Kadena.Models.CreditCard;
using System;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface ISubmissionIdProvider
    {
        void SaveSubmission(Submission submission);
        Submission GetSubmission(Guid submissionId);
        IEnumerable<Submission> GetSubmissions(int siteId, int userId, int customerId);
        void DeleteSubmission(Guid submissionId);
        void UpdateSubmissionId(Guid oldId, Guid newId);
    }
}
