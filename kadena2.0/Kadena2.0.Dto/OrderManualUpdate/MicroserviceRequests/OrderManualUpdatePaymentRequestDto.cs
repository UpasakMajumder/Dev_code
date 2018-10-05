using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.Dto.OrderManualUpdate.MicroserviceRequests
{
    public class OrderManualUpdatePaymentRequestDto: OrderManualUpdateRequestDto
    {
        public PaymentOptionsDto PaymentOption { get; set; }
    }
}
