using System.Collections.Generic;

namespace Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests
{
    public class AddressDto
    {
        public List<string> StreetLines { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postal { get; set; }
        public string Country { get; set; }
    }
}
