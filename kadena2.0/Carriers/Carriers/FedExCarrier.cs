using System.Collections.Generic;
using System.Linq;
using CMS;
using Kadena2.Carriers;

[assembly: RegisterCustomClass("FedExCarrier", typeof(FedExCarrier))]

namespace Kadena2.Carriers
{
    public class FedExCarrier : CarrierBase
    {
        public FedExCarrier() : base()
        {
            ProviderApiKey = "Fedex";
            CarrierProviderName = "FedEx";
        }

        public override List<KeyValuePair<string, string>> GetServices()
        {
            var services = new SortedDictionary<string, string>
            {
                { "FEDEX_2_DAY","2nd Day" },
                { "FIRST_OVERNIGHT","First Overnight" },
                { "FEDEX_3_DAY_FREIGHT","3 Day" },
                { "FEDEX_EXPRESS_SAVER","Express Saver" },
                { "FEDEX_GROUND","Ground" },
                { "STANDARD_OVERNIGHT","Standard Overnight" },
                { "PRIORITY_OVERNIGHT","Priority Overnight" },
                { "PRIORITY_OVERNIGHT#","Priority Overnight (10:30am)" },
                { "PRIORITY_OVERNIGHT##","Priority Overnight(Saturday)" },
                { "INTERNATIONAL_ECONOMY","Int'l Economy" },
                { "INTERNATIONAL_ECONOMY#","International Ground" },
                { "INTERNATIONAL_ECONOMY##","Ground(Canada Only)" },
                { "INTERNATIONAL_ECONOMY###","Int'l Ground" },
                { "INTERNATIONAL_PRIORITY","Int'l Priority" }
            };

            return services.ToList();
        }
    }
}
