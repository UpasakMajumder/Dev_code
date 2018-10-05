using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kadena.Models.Orders
{
    public class OrderUpdatePayment: OrderUpdate
    {
        public Kadena.Models.SubmitOrder.PaymentMethod PaymentMethod { get; set; }
    }
}
