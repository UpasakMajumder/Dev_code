using System;

namespace Kadena.WebAPI.Contracts
{
    public interface ICreditCardService
    {
        Guid GenerateSubmissionId();
        bool VerifySubmissionId(string submissionId);

        // TODO bool SaveToken(string submissionId, string token);
    }
}