using System.Collections.Generic;
using System.Linq;
using CMS;
using Kadena2.Carriers;

[assembly: RegisterCustomClass("UPSCustomerCarrier", typeof(UPSCustomerCarrier))]

namespace Kadena2.Carriers
{ 
    public class UPSCustomerCarrier : CarriersCustomersBase
    {
        public UPSCustomerCarrier() : base()
        {
            CarrierProviderName = "UPS";
        }
    }
}
