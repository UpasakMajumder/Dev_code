using Kadena.Models.CreditCard;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ICreditCardService
    {
        Submission GenerateSubmissionId();
        bool VerifySubmissionId(string submissionId);
        Task<bool> SaveToken(SaveTokenData tokenData);
        bool CreditcardSaved(string submissionId);
    }
}