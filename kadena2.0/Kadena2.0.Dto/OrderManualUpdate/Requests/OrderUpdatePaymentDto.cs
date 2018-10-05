using Kadena.Dto.SubmitOrder.Requests;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.OrderManualUpdate.Requests
{
    public class OrderUpdatePaymentDto
    {
        [Required]
        public string OrderId { get; set; }

        [Required]
        public PaymentMethodDto PaymentMethod { get; set; }

    }
}
