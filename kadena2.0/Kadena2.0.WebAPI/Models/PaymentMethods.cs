using System.Collections.Generic;

namespace Kadena.WebAPI.Models
{
    public class PaymentMethods
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<PaymentMethod> Items { get; set; }
    }
}