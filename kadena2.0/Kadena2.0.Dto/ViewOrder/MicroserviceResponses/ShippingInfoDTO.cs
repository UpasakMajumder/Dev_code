using System;

namespace Kadena.Dto.ViewOrder.MicroserviceResponses
{
    public class ShippingInfoDTO
    {
        public string Provider { get; set; }
        public string AddressTo { get; set; }
        public string AddressFrom { get; set; }
        public DateTime ShippingDate { get; set; }
    }
}
