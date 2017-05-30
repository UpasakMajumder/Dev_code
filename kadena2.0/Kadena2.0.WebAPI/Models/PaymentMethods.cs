using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Models
{
    public class PaymentMethods
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<PaymentMethod> Items { get; set; }
    }
}