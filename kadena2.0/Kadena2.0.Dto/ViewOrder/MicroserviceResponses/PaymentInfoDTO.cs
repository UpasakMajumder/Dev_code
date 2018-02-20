using System;

namespace Kadena.Dto.ViewOrder.MicroserviceResponses
{
    public class PaymentInfoDTO
    {
        public string PaymentMethod { get; set; }
        public DateTime? CapturedDate { get; set; }
        public double Summary { get; set; }
        public double Shipping { get; set; }
        public double Tax { get; set; }
    }
}
