using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.OrderManualUpdate.Requests
{
    public class OrderPaymentUpdateDto
    {
        [Required]
        public string OrderId { get; set; }



    }
}
