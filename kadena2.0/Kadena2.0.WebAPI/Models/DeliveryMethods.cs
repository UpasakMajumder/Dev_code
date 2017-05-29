using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Models
{
    public class DeliveryMethods
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<DeliveryMethod> items { get; set; }
    }
}