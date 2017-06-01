using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Infrastructure.Requests
{
    public class PaymentMethodDto
    {
        public int Id { get; set; }
        public string Invoice { get; set; }
    }
}