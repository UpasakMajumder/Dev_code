using System.Collections.Generic;
using System.Linq;
using CMS;
using Kadena2.Carriers;

[assembly: RegisterCustomClass("FedExCustomerCarrier", typeof(FedExCustomerCarrier))]

namespace Kadena2.Carriers
{
    public class FedExCustomerCarrier : CarriersCustomersBase
    {
        public FedExCustomerCarrier() : base()
        {
            CarrierProviderName = "FedEx";
        }        
    }
}
