using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Models.SubmitOrder
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Invoice { get; set; }
    }
}