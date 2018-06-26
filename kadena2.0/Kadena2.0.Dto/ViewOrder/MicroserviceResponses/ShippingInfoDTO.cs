using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using System;

namespace Kadena.Dto.ViewOrder.MicroserviceResponses
{
    public class ShippingInfoDTO
    {
        public string Provider { get; set; }
        public int ShippingOptionId { get; set; }
        public string CarrierCode { get; set; }
        public string ShippingService { get; set; }
        public AddressDTO AddressTo { get; set; }
        public string AddressFrom { get; set; }
        public DateTime? ShippingDate { get; set; }
    }
}
