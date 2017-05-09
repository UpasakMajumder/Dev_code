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

        public override List<KeyValuePair<string, string>> GetServices()
        {
            var services = new SortedDictionary<string, string>
            {
                {"PRIORITY_OVERNIGHT", "Priority overnight"},
                {"STANDARD_OVERNIGHT", "Standard overnight"},
                {"FEDEX_2_DAY", "Fedex 2nd day"}
            };

            return services.ToList();
        }
    }
}
