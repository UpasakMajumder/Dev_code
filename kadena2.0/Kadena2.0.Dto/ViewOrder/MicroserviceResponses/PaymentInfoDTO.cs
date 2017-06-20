namespace Kadena.Dto.ViewOrder.MicroserviceResponses
{
    public class PaymentInfoDTO
    {
        public string PaymentMethod { get; set; }
        public int Summary { get; set; }
        public int Shipping { get; set; }
        public int Tax { get; set; }
    }
}
