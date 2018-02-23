using Kadena.Models.CreditCard;
using System;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ISubmissionService
    {
        Submission GenerateNewSubmission(string orderJson = "");
        Guid GenerateNewSubmissionId();
        bool VerifySubmissionId(string submissionId);
        Submission GetSubmission(string submissionId);
        void SetAsProcessed(Submission submission, bool orderSuccesss, string redirectUrl, string error = "");
        void DeleteProcessedSubmission(Submission submission);
        void SetSaveCardJson(string submissionId, string saveCardJson);
        bool CheckOwner(Submission submission);
        Guid RenewSubmission(string submissionId);
    }
}