using Kadena.Dto.Checkout;
using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.SubmitOrder.Requests
{
    public class SubmitRequestDto
    {
        [Required]
        public DeliveryAddressDTO DeliveryAddress { get; set; }
        public int DeliveryMethod { get; set; }
        public PaymentMethodDto PaymentMethod { get; set; }
        public bool AgreeWithTandC { get; set; }
    }
}