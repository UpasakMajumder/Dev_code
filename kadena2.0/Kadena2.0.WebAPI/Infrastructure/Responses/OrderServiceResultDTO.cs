using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Infrastructure.Responses
{
    public class OrderServiceResultDTO
    {
        public bool Success { get; set; }
        public object Payload { get; set; }
        public string ErrorMessage { get; set; }
    }
}