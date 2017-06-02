using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Models.SubmitOrder
{
    public class SubmitOrderResult
    {
        public string RedirectURL { get; set; }
        public string OrderId { get; set; }
    }
}