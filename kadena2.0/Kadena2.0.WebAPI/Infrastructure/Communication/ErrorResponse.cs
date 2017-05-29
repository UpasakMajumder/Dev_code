using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Infrastructure.Communication
{
    public class ErrorResponse : BaseResponse<string>
    {
        public ErrorResponse(string message)
        {
            Success = false;
            Payload = null;
            ErrorMessage = message;
        }
    }
}