using Kadena.Models.CreditCard;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ISubmissionService
    {
        Submission GenerateSubmissionId();
        bool VerifySubmissionId(string submissionId);
    }
}