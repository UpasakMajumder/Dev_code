using System.Collections.Generic;

namespace Kadena.WebAPI.Models
{
    public class DeliveryMethods
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<DeliveryMethod> items { get; set; }
    }
}