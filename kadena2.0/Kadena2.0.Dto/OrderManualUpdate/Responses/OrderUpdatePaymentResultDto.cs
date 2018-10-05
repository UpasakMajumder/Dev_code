using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.Dto.OrderManualUpdate.Responses
{
    public class OrderUpdatePaymentResultDto
    {
        public bool Success { get; set; }
        public string RedirectURL { get; set; }
        public string Error { get; set; }
    }
}
