using System.Collections.Generic;
using System.Linq;
using CMS;
using Kadena2.FedExCarrier;

[assembly: AssemblyDiscoverable]
[assembly: RegisterCustomClass("FedExCarrier", typeof(FedExCarrier))]

namespace Kadena2.FedExCarrier
{
    public class FedExCarrier : CarrierBase.CarrierBase
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
                {"PRIORITY_OVERNIGHT", "Priority overnight"},
                {"STANDARD_OVERNIGHT", "Standard overnight"},
                {"FEDEX_2_DAY", "Fedex 2nd day"}
            };

            return services.ToList();
        }
    }
}
