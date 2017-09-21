using System;

namespace Kadena.WebAPI.Contracts
{
    public interface ICreditCardService
    {
        Guid GenerateSubmissionId();
        bool VerifySubmissionId(Guid submissionId);
    }
}