using System;
using System.Collections.Generic;
using System.Linq;
using Kadena.Models.Checkout;

namespace Kadena.Models.OrderDetail
{

    public class OrderDetailPaymentMethod
    {
        public OrderDetailPaymentMethod()
        {
            CheckedObj = new PaymentMethodSelected();
        }

        public PaymentMethods Ui { get; set; }
        public PaymentMethodSelected CheckedObj { get; set; }
    }
}
