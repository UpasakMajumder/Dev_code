using Kadena.Models.CreditCard;
using System;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Contracts
{
    public interface ICreditCardService
    {
        Guid GenerateSubmissionId();
        bool VerifySubmissionId(string submissionId);
        Task<bool> SaveToken(SaveTokenData tokenData);
    }
}