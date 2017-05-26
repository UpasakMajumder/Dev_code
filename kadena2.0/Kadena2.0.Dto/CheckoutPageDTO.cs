using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena2._0.Dto
{
    public class CheckoutPageDTO
    {
        public DeliveryAddressesContainerDTO DeliveryAddresses { get; set; }
        public DeliveryMethodContainerDTO DeliveryMethod { get; set; }
        public PaymentMethodContainerDTO PaymentMethod { get; set; }
        public TotalsContainerDTO Totals { get; set; }
        public string SubmitLabel { get; set; }
    }
}
