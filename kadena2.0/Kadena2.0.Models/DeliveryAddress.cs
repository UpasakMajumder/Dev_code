using System.Collections.Generic;

namespace Kadena.Models
{
    public class DeliveryAddress
    {
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string Zip { get; set; }
        public int Id { get; set; }
        public bool Checked { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}