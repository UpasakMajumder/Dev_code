using System.Collections.Generic;

namespace Kadena.WebAPI.Models
{
    public class DeliveryAddress
    {
        public List<string> Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int Id { get; set; }
        public bool Checked { get; set; }
    }
}