using System;

namespace Kadena.BusinessLogic.Services.Approval
{
    public class ApprovalServiceException : Exception
    {
        public ApprovalServiceException(string message) : base(message)
        {
        }
    }
}
