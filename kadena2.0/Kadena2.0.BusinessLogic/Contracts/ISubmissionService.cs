using Kadena.Models.CreditCard;
using System;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ISubmissionService
    {
        Submission GenerateNewSubmission(string orderJson = "");
        bool VerifySubmissionId(string submissionId);
        Submission GetSubmission(string submissionId);
        void SetAsProcessed(Submission submission, string redirectUrl);
        void DeleteProcessedSubmission(Submission submission);
    }
}