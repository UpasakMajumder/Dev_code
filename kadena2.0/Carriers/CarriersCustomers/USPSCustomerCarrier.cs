using System.Collections.Generic;
using System.Linq;
using CMS;
using Kadena2.Carriers;

[assembly: RegisterCustomClass("USPSCustomerCarrier", typeof(USPSCustomerCarrier))]

namespace Kadena2.Carriers
{
    public class USPSCustomerCarrier : CarriersCustomersBase
    {
        public USPSCustomerCarrier() : base()
        {
            CarrierProviderName = "USPS";
        }
    }
}
