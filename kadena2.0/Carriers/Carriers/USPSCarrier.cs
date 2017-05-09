using System.Collections.Generic;
using System.Linq;
using CMS;
using Kadena2.Carriers;

[assembly: RegisterCustomClass("USPSCarrier", typeof(USPSCarrier))]

namespace Kadena2.Carriers
{
    public class USPSCarrier : CarrierBase
    {
        public USPSCarrier() : base()
        {
            ProviderApiKey = "USPS";
            CarrierProviderName = "USPS";
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
