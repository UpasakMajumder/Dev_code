using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Models.SubmitOrder
{
    public class SubmitOrderResult
    {
        public string Payload { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
}